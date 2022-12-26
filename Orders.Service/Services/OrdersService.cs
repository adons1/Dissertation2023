using Core;
using Core.RedisKeys;
using Customers.TransportTypes.TransportServices.Contracts;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Orders.Service.Models;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using Orders.Service.Provider.ProviderContracts;
using Orders.Service.TransportTypes.TransportServices.Contracts;
using Orders.TransportTypes.TransportModels;
using OrderTransport = Orders.TransportTypes.TransportModels.Order;
using Products.TransportTypes.TransportModels;
using Products.TransportTypes.TransportServices.Contracts;
using AutoMapper;
using StackExchange.Redis;
using System.Linq;
using Core.Exceptions;
using System;

namespace Orders.Service.Services;

public class OrdersService : IOrdersService, IOauthServiceAuthorize
{
    IMapper _mapper;
    ICustomersService _customersService;
    IOauthService _oauthService;
    IProductsService _productsService;
    IOrdersProvider _ordersProvider;
    IObtainCustomerIdentity _obtainIdentity;
    IDistributedCache _distributedCache;
    IHttpContextAccessor _httpContext;

    public OrdersService(
        IMapper mapper,
        ICustomersService customersService,
        IOauthService oauthService,
        IProductsService productsService,
        IOrdersProvider ordersProvider,
        IDistributedCache distributedCache,
        IObtainCustomerIdentity obtainIdentity,
        IHttpContextAccessor httpContext)
    {
        _mapper = mapper;
        _customersService = customersService;
        _oauthService = oauthService;
        _productsService = productsService;
        _ordersProvider = ordersProvider;
        _obtainIdentity = obtainIdentity;
        _distributedCache = distributedCache;
        _httpContext = httpContext;
    }

    public async Task<Result<bool>> Cancel(Guid id)
    {
        var result = _ordersProvider.DeleteById(id);
        return await Task.FromResult(new SuccessResult<bool>(result));
    }

    public async Task<Result<OrderTransport>> SelectById(Guid id)
    {
        var identityGuid = await _obtainIdentity.Identity();
        var identity = await _customersService.SelectById(identityGuid);

        var requestedOrders = _ordersProvider.SelectById(id);
        if (requestedOrders == null || requestedOrders.Count() == 0)
            return new FailureResult<OrderTransport>("Order not found");

        var ordersGroupped = requestedOrders.GroupBy(x => x.OrderId).FirstOrDefault();
        var allProducts = (await _productsService.SelectByIds(requestedOrders.Select(x => x.ProductId)))?.Payload
            ?.Select(x => _mapper.Map<Product, OrderedProduct>(x));

        var orderTransport = new OrderTransport
        {
            Id = ordersGroupped.Key,
            Customer = identity.Payload,
            Products = from p in ordersGroupped
                       let product = allProducts?.SingleOrDefault(x => x.Id == p.ProductId)
                       where product != null
                       select product
        };

        return new SuccessResult<OrderTransport>(orderTransport);
    }

    public async Task<Result<IEnumerable<OrderTransport>>> SelectAll()
    {
        var identityGuid = await _obtainIdentity.Identity();
        var identity = await _customersService.SelectById(identityGuid);

        var orders = _ordersProvider.SelectAllForUser(identity.Payload.Id);
        if(orders == null || orders.Count() == 0)
            return new SuccessResult<IEnumerable<OrderTransport>>(null);
        
        var ordersGroupped = orders.GroupBy(x => x.OrderId);
        var allProducts = (await _productsService.SelectByIds(orders.Select(x => x.ProductId)))?.Payload
            ?.Select(x => _mapper.Map<Product, OrderedProduct>(x));

        var ordersTransport = new List<OrderTransport>();
        foreach(var order in ordersGroupped)
        {
            ordersTransport.Add(new OrderTransport
            {
                Id = order.Key,
                Customer = identity.Payload,
                Products = from p in order
                           let product = allProducts?.SingleOrDefault(x => x.Id == p.ProductId)
                           where product != null
                           select product
            });
        }

        return new SuccessResult<IEnumerable<OrderTransport>>(ordersTransport);
    }

    public async Task<Result<TokenModel>> Token(string code)
    {
        var token = await _oauthService.Token(code);

        await _distributedCache.SetStringAsync(CacheKeys.ServiceTokenByToken(token.Payload.Token), JsonConvert.SerializeObject(token.Payload));

        return token;
    }

    public async Task<Result<OrderTransport>> Create(IEnumerable<Guid> productsId)
    {
        var identityGuid = await _obtainIdentity.Identity();
        var identity = await _customersService.SelectById(identityGuid);

        var products = (await _productsService.SelectByIds(productsId))?.Payload;

        var totalPrice = products.Sum(x => x.Price);
        //if (identity.Payload.Account < totalPrice)
        //    return new FailureResult<OrderTransport>("Not enough funds");

        //var emptyProduct = products.Where(x => x.Quantity == 0);
        //if (emptyProduct.Count() == 0)
        //    return new FailureResult<OrderTransport>($"Not enough products {string.Join(", ", emptyProduct.Select(x => x.Id))}");

        var createdOrdersIds = new List<Guid?>();
        Guid? orderId = null;
        foreach (var productId in productsId)
        {
            orderId = _ordersProvider.CreateById(orderId, identity.Payload.Id, productId);
            createdOrdersIds.Add(orderId);
        }

        if(!orderId.HasValue)
            return new FailureResult<OrderTransport>("Error while creating an order");

        try
        {
            await _customersService.Waste(productsId, totalPrice);
        }
        catch (RollbackException rollback)
        {
            foreach(var id in createdOrdersIds)
            {
                if(id.HasValue) _ordersProvider.DeleteById((Guid)id);
            }
            return new FailureResult<OrderTransport>();
        }

        var order = (await SelectById(orderId.Value))?.Payload;
        
        return new SuccessResult<OrderTransport>(order);
    }
}

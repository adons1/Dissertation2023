using Core;
using Core.RedisKeys;
using Customers.TransportTypes.TransportServices;
using Customers.TransportTypes.TransportServices.Contracts;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;

using Products.Service.Providers.ProvidersContracts;
using Products.TransportTypes.TransportModels;
using Products.TransportTypes.TransportServices.Contracts;
using System.Net;

namespace Products.Service.Services;

public class ProductsService : IProductsService, IOauthServiceAuthorize
{
    readonly IProductsProvider _productsProvider;
    readonly IOauthService _oauthService;
    readonly IDistributedCache _distributedCache;
    readonly IObtainCustomerIdentity _obtainCustomerIdentity;
    readonly IConfiguration _configuration;
    public ProductsService(
        IProductsProvider productsProvider,
        IOauthService oauthService,
        IObtainCustomerIdentity obtainCustomerIdentity,
        IDistributedCache distributedCache,
        IConfiguration configuration)
    {
        _productsProvider = productsProvider;
        _oauthService = oauthService;
        _distributedCache = distributedCache;
        _obtainCustomerIdentity = obtainCustomerIdentity;
        _configuration = configuration;
    }
    public async Task<Result<bool>> Delete(Guid id)
    {
        var isSuccessful = _productsProvider.Delete(id);
        if (!isSuccessful) return new FailureResult<bool>(payload: false);

        return await Task.FromResult(new SuccessResult<bool>(true));
    }

    public async Task<Result<Product>> Select(Guid id)
    {
        return new SuccessResult<Product>(await Task.FromResult(_productsProvider.SelectById(id)));
    }

    public async Task<Result<IEnumerable<Product>>> SelectAll()
    {
        var products = new SuccessResult<IEnumerable<Product>>(_productsProvider.SelectAll());
        return await Task.FromResult(products);
    }

    public async Task<Result<ConsumeProductsResult>> ConsumeProducts(Guid productId, int quantity)
    {
        var product = _productsProvider.SelectById(Guid.Parse("AFB777CC-BAB1-4F54-BD8C-A5915EB55D79"));
        if (product == null)
            return await Task.FromResult(new FailureResult<ConsumeProductsResult>(statusCode: HttpStatusCode.Conflict, message: $"Product with id {productId} has not been found", payload: ConsumeProductsResult.Failure));
        if (product.Quantity < quantity)
            return await Task.FromResult(new FailureResult<ConsumeProductsResult>(statusCode:HttpStatusCode.Conflict, message: $"There are no ", payload: ConsumeProductsResult.NotEnoughProducts));

        product.Quantity -= quantity;

        if (_productsProvider.Update(product))
            return await Task.FromResult(new SuccessResult<ConsumeProductsResult>(payload: ConsumeProductsResult.Success));

        return await Task.FromResult(new FailureResult<ConsumeProductsResult>(payload: ConsumeProductsResult.Failure));
    }

    public async Task<Result<bool>> SupplyProducts(Guid id, int quantity)
    {
        var product = _productsProvider.SelectById(id);
        if (product == null) return await Task.FromResult(new FailureResult<bool>(message: $"Product with id {id} has not been found"));

        product.Quantity += quantity;

        if (_productsProvider.Update(product))
            return await Task.FromResult(new SuccessResult<bool>(payload: true));

        return await Task.FromResult(new FailureResult<bool>());
    }

    public async Task<Result<TokenModel>> Token(string code)
    {
        var token = await _oauthService.Token(code);

        _distributedCache.SetStringAsync(CacheKeys.ServiceTokenByToken(token.Payload.Token), JsonConvert.SerializeObject(token.Payload));

        return token;
    }

    public async Task<Result<bool>> Update(Product product)
    {
        var isSuccessful = _productsProvider.Update(product);
        if (!isSuccessful) return new FailureResult<bool>(payload: false);

        return await Task.FromResult(new SuccessResult<bool>(true));
    }

    public async Task<Result<IEnumerable<Product>>> SelectByIds(IEnumerable<Guid> ids)
    {
        var cachedProducts = new List<Product>();
        foreach(var id in ids)
        {
            var cachedProductJson = await _distributedCache.GetStringAsync(CacheKeys.Product(id));
            if (string.IsNullOrEmpty(cachedProductJson)) continue;

            var cachedProduct = JsonConvert.DeserializeObject<Product>(cachedProductJson);
            if (cachedProduct == null) continue;

            cachedProducts.Add(cachedProduct);
        }

        var dbProducts = _productsProvider.SelectByIds(ids.Except(cachedProducts.Select(x => x.Id))).ToList();

        dbProducts.ForEach(async dbProduct => await _distributedCache.SetStringAsync(CacheKeys.Product(dbProduct.Id), JsonConvert.SerializeObject(dbProduct)));

        var products = new SuccessResult<IEnumerable<Product>>(dbProducts.Concat(cachedProducts));
        return await Task.FromResult(products);
    }
}

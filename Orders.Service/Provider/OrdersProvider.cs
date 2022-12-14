using AutoMapper;
using Orders.Service.Data;
using Orders.Service.Models;
using Orders.Service.Provider.ProviderContracts;
using OrderTransport = Orders.TransportTypes.TransportModels.Order;

namespace Orders.Service.Provider;

public class OrdersProvider : IOrdersProvider
{
    readonly IMapper _mapper;
    readonly OrdersDbContext _ordersContext;
    public OrdersProvider(OrdersDbContext oredersContext, IMapper mapper)
    {
        _mapper = mapper;
        _ordersContext = oredersContext;
    }

    public IEnumerable<Order> SelectAllForUser(Guid userId)
    {
        return (from o in _ordersContext.Orders
               where o.CustomerId == userId
               select o).ToList();
    }

    public IEnumerable<Order> SelectAll()
    {
        return _ordersContext.Orders.ToList();
    }
    public IEnumerable<Order> SelectById(Guid orderId)
    {
        return _ordersContext.Orders.Where(o => o.OrderId == orderId);
    }

    public bool DeleteById(Guid orderId)
    {
        var removingOrders = from order in _ordersContext.Orders
                             where order.OrderId == orderId
                             select order;

        _ordersContext.Orders.RemoveRange(removingOrders);

        _ordersContext.SaveChanges();

        return true;
    }

    public Guid CreateById(Guid? orderId, Guid customerId, Guid productId)
    {
        var newOrderId = orderId ?? Guid.NewGuid();

        _ordersContext.Orders.Add(new Order { 
            OrderId= newOrderId,
            CustomerId = customerId,
            ProductId = productId 
        });

        _ordersContext.SaveChanges();

        return newOrderId;
    }
}

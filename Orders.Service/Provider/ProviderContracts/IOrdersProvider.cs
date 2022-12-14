using Orders.Service.Models;

namespace Orders.Service.Provider.ProviderContracts;

public interface IOrdersProvider
{
    IEnumerable<Order> SelectAllForUser(Guid userId);
    IEnumerable<Order> SelectAll();
    IEnumerable<Order> SelectById(Guid orderId);
    bool DeleteById(Guid orderId);
    Guid CreateById(Guid? orderId, Guid customerId, Guid productId);
}

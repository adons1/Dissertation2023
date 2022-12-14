using Products.Service.Models;
using ProductTransport = Products.TransportTypes.TransportModels.Product;

namespace Products.Service.Providers.ProvidersContracts;

public interface IProductsProvider
{
    ProductTransport? SelectById(Guid id);
    IEnumerable<ProductTransport?> SelectAll(IEnumerable<Guid>? ids = null);
    IEnumerable<ProductTransport?> SelectByIds(IEnumerable<Guid> ids);
    bool Create(ProductTransport product);
    bool Update(ProductTransport product);
    bool Delete(Guid guid);
}

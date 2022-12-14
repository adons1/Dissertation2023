using Core;
using Products.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.TransportTypes.TransportServices.Contracts;

public interface IProductsService
{
    public Task<Result<IEnumerable<Product>>> SelectAll();
    public Task<Result<IEnumerable<Product>>> SelectByIds(IEnumerable<Guid> ids);
    public Task<Result<Product>> Select(Guid id);
    public Task<Result<bool>> Delete(Guid id);
    public Task<Result<bool>> Update(Product product);
    public Task<Result<ConsumeProductsResult>> ConsumeProducts(Guid id, int quantity);
    public Task<Result<bool>> SupplyProducts(Guid id, int quantity);
}

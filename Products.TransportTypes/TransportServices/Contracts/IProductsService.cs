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
    public Task<Result<Product>> Select();
    public Task<Result<bool>> Delete();
    public Task<Result<bool>> Update();
}

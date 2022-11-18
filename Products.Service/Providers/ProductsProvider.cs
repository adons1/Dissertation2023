using AutoMapper;
using Products.Service.Data;
using Products.Service.Providers.ProvidersContracts;
using Products.TransportTypes.TransportModels;
using Product = Products.Service.Models.Product;
using ProductTransport = Products.TransportTypes.TransportModels.Product;
namespace Products.Service.Providers;

public class ProductsProvider : IProductsProvider
{
    readonly IMapper _mapper;
    readonly ProductsDbContext _productContext;
    public ProductsProvider(IMapper mapper, ProductsDbContext productContext)
    {
        _mapper = mapper;
        _productContext = productContext;
    }

    public bool Create(ProductTransport product)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Guid guid)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ProductTransport?> SelectAll(IEnumerable<Guid>? excludeIds = null)
    {
        return from product in _productContext.Products
               where (excludeIds ?? new List<Guid>()).All(guid => guid != product.Id)
               select _mapper.Map<Product, ProductTransport>(product);
    }

    public ProductTransport? SelectById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool Update(ProductTransport product)
    {
        throw new NotImplementedException();
    }
}

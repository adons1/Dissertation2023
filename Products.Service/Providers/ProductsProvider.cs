using AutoMapper;
using Products.Service.Data;
using Products.Service.Models;
using Products.Service.Providers.ProvidersContracts;
using Products.TransportTypes.TransportModels;
using Product = Products.Service.Models.Product;
using ProductTransport = Products.TransportTypes.TransportModels.Product;
namespace Products.Service.Providers;

public class ProductsProvider : IProductsProvider
{
    readonly IMapper _mapper;
    readonly ProductsDbContext _productsContext;
    public ProductsProvider(IMapper mapper, ProductsDbContext productsContext)
    {
        _mapper = mapper;
        _productsContext = productsContext;
    }

    public bool Create(ProductTransport product)
    {
        var newProduct = _mapper.Map<ProductTransport, Product>(product);
        newProduct.Id= Guid.NewGuid();

        _productsContext.Products.Add(newProduct);
        _productsContext.SaveChanges();

        return true;
    }

    public bool Delete(Guid id)
    {
        var product = _productsContext.Products
            .SingleOrDefault(c => c.Id == id);

        if (product == null) return false;

        _productsContext.Products.Remove(product);
        _productsContext.SaveChanges();

        return true;
    }

    public IEnumerable<ProductTransport?> SelectAll(IEnumerable<Guid>? excludeIds = null)
    {
        return from product in _productsContext.Products
               where (excludeIds ?? new List<Guid>()).All(guid => guid != product.Id)
               select _mapper.Map<Product, ProductTransport>(product);
    }

    public ProductTransport? SelectById(Guid id)
    {
        var product = _productsContext.Products
            .SingleOrDefault(c => c.Id == id);

        if(product == null) return null;

        return _mapper.Map<Product, ProductTransport>(product);
    }

    public IEnumerable<ProductTransport?> SelectByIds(IEnumerable<Guid> ids)
    {
        return from p in _productsContext.Products
        where ids.Contains(p.Id)
               select _mapper.Map<Product, ProductTransport>(p);
    }

    public bool Update(ProductTransport product)
    {
        _productsContext.Update(product);
        _productsContext.SaveChanges();

        return true;
    }
}

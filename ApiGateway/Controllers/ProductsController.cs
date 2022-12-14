using Core;
using Customers.TransportTypes;
using Customers.TransportTypes.Attributes;
using Microsoft.AspNetCore.Mvc;
using Products.TransportTypes.TransportModels;
using Products.TransportTypes.TransportServices.Contracts;
using ProductTransport = Products.TransportTypes.TransportModels.Product;

namespace ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : Controller
{
    readonly IProductsService _productsService;
    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet, Route("select_all"), AuthorizeClient]
    public async Task<Result<IEnumerable<ProductTransport>>> SelectAll()
    {
        return await _productsService.SelectAll();
    }

    [HttpGet, Route("delete"), AuthorizeClient]
    public async Task<Result<bool>> Delete(Guid id)
    {
        return await _productsService.Delete(id);
    }

    [HttpPost, Route("update"), AuthorizeClient]
    public async Task<Result<bool>> Update(ProductTransport product)
    {
        return await _productsService.Update(product);
    }
    [HttpGet, Route("select_by_id"), AuthorizeClient]
    public async Task<Result<ProductTransport>> Select(Guid id)
    {
        return await _productsService.Select(id);
    }

    [HttpGet, Route("consume"), AuthorizeClient]
    public async Task<Result<ConsumeProductsResult>> ConsumeProducts(Guid id, int quantity)
    {
        if (quantity <= 0)
            return new FailureResult<ConsumeProductsResult>(message: "quantity has to be more than 0", payload: ConsumeProductsResult.Failure);

        return await _productsService.ConsumeProducts(id, quantity);
    }

    [HttpGet, Route("supply"), AuthorizeClient]
    public async Task<Result<bool>> SupplyProducts(Guid id, int quantity)
    {
        if (quantity <= 0)
            return new FailureResult<bool>(message: "quantity has to be more than 0", payload: false);

        return await _productsService.SupplyProducts(id, quantity);
    }
}

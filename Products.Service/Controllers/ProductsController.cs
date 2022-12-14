using Core;
using Microsoft.AspNetCore.Mvc;
using OauthAuthorization.TransportTypes.Attributes;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using Products.TransportTypes.TransportModels;
using ProductTransport = Products.TransportTypes.TransportModels.Product;
using Products.TransportTypes.TransportServices.Contracts;
using Products.Service.Models;
using Customers.TransportTypes;

namespace Products.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
[AuthorizeService]
public class ProductsController : Controller, IProductsService
{
    readonly IProductsService _productsService;
    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet, Route("select_all")]
    public async Task<Result<IEnumerable<ProductTransport>>> SelectAll()
    {
        return await _productsService.SelectAll();
    }

    [HttpGet, Route("delete")]
    public async Task<Result<bool>> Delete(Guid id)
    {
        return await _productsService.Delete(id);
    }

    [HttpPost, Route("update")]
    public async Task<Result<bool>> Update(ProductTransport product)
    {
        return await _productsService.Update(product);
    }
    [HttpGet, Route("select_by_id")]
    public async Task<Result<ProductTransport>> Select(Guid id)
    {
        return await _productsService.Select(id);
    }

    [HttpGet, Route("consume")]
    public async Task<Result<ConsumeProductsResult>> ConsumeProducts(Guid id, int quantity)
    {
        if (quantity <= 0)
            return new FailureResult<ConsumeProductsResult>(message: "quantity has to be more than 0", payload: ConsumeProductsResult.Failure);

        return await _productsService.ConsumeProducts(id, quantity);
    }

    [HttpGet, Route("supply")]
    public async Task<Result<bool>> SupplyProducts(Guid id, int quantity)
    {
        if (quantity <= 0)
            return new FailureResult<bool>(message: "quantity has to be more than 0", payload: false);

        return await _productsService.SupplyProducts(id, quantity);
    }

    [HttpPost, Route("select_by_ids")]
    public async Task<Result<IEnumerable<ProductTransport>>> SelectByIds([FromBody] IEnumerable<Guid> ids)
    {
        if (ids == null || ids.Count() < 1)
            return new FailureResult<IEnumerable<ProductTransport>>(message: "ids in null or empty");

        return await _productsService.SelectByIds(ids);
    }
}

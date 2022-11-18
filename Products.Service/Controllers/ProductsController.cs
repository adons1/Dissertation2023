using Core;
using Microsoft.AspNetCore.Mvc;
using OauthAuthorization.TransportTypes.Attributes;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using Products.TransportTypes.TransportModels;
using Products.TransportTypes.TransportServices.Contracts;

namespace Products.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : Controller, IProductsService
{
    readonly IProductsService _productsService;
    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet, Route("select_by_id")]
    public Task<Result<Product>> Select()
    {
        throw new NotImplementedException();
    }

    [HttpGet, Route("select_all"), Authorize]
    public async Task<Result<IEnumerable<Product>>> SelectAll()
    {
        return await _productsService.SelectAll();
    }

    [HttpPost, Route("delete")]
    public Task<Result<bool>> Delete()
    {
        throw new NotImplementedException();
    }

    [HttpPost, Route("update")]
    public Task<Result<bool>> Update()
    {
        throw new NotImplementedException();
    }
}

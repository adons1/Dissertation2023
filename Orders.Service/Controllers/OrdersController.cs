using Core;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using Orders.Service.TransportTypes.TransportServices.Contracts;
using Products.TransportTypes.TransportModels;
using Products.TransportTypes.TransportServices.Contracts;

namespace Orders.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : Controller, IOrdersService
{
    ICustomersService _customersService;
    IProductsService _productsService;
    public OrdersController(ICustomersService customersService, IProductsService productsService)
    {
        _customersService = customersService;
        _productsService = productsService;
    }
    readonly IOrdersService _ordersService;
    [HttpGet, Route("select_all_customers")]
    public async Task<Result<IEnumerable<Customer>>> Index()
    {
        return await _customersService.SelectAll();
    }
    [HttpGet, Route("select_all_products")]
    public async Task<Result<IEnumerable<Product>>> SelectAllProducts()
    {
        return await _productsService.SelectAll();
    }
}

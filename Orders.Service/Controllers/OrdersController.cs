using Core;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using Orders.Service.TransportTypes.TransportServices.Contracts;

namespace Orders.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : Controller, IOrdersService
{
    ICustomersService _customersService;
    public OrdersController(ICustomersService customersService)
    {
        _customersService = customersService;
    }
    readonly IOrdersService _ordersService;
    public async Task<Result<IEnumerable<Customer>>> Index()
    {
        return await _customersService.SelectAll();
    }
}

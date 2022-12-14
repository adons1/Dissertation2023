using Core;
using Customers.TransportTypes.TransportServices.Contracts;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using OauthAuthorization.TransportTypes.Attributes;
using Orders.Service.Services;
using Orders.Service.TransportTypes.TransportServices.Contracts;
using Orders.TransportTypes.TransportModels;
using Products.TransportTypes.TransportModels;
using Products.TransportTypes.TransportServices.Contracts;
using System.CodeDom;

namespace Orders.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
[AuthorizeService]
public class OrdersController : Controller, IOrdersService
{ 
    readonly IOrdersService _ordersService;
    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpGet, Route("cancel")]
    public async Task<Result<bool>> Cancel(Guid id)
    {
        return await _ordersService.Cancel(id);
    }

    [HttpGet, Route("select_all")]
    public async Task<Result<IEnumerable<Order>>> SelectAll()
    {
        return await _ordersService.SelectAll();
    }

    [HttpGet, Route("select_by_id")]
    public async Task<Result<Order>> SelectById(Guid id)
    {
        return await _ordersService.SelectById(id);
    }

    [HttpPost, Route("create")]
    public async Task<Result<Order>> Create([FromBody] IEnumerable<Guid> ids)
    {
        return await _ordersService.Create(ids);
    }
}

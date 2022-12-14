using Core;
using Customers.TransportTypes;
using Customers.TransportTypes.Attributes;
using Microsoft.AspNetCore.Mvc;
using OauthAuthorization.TransportTypes.Attributes;
using Orders.Service.TransportTypes.TransportServices.Contracts;
using Orders.TransportTypes.TransportModels;
using Products.TransportTypes.TransportModels;

namespace ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : Controller, IOrdersService
{
    readonly IOrdersService _ordersService;
    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }
    [HttpGet, Route("cancel"), AuthorizeClient]
    public async Task<Result<bool>> Cancel(Guid id)
    {
        return await _ordersService.Cancel(id);
    }

    [HttpPost, Route("create"), AuthorizeClient]
    public async Task<Result<Order>> Create([FromBody] IEnumerable<Guid> ids)
    {
        return await _ordersService.Create(ids);
    }

    [HttpGet, Route("select_all"), AuthorizeClient]
    public async Task<Result<IEnumerable<Order>>> SelectAll()
    {
        return await _ordersService.SelectAll();
    }

    [HttpGet, Route("select_by_id"), AuthorizeClient]
    public async Task<Result<Order>> SelectById(Guid id)
    {
        return await _ordersService.SelectById(id);
    }
}

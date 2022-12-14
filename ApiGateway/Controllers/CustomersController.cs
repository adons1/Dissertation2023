using Core;
using Customers.TransportTypes.TransportModels;
using Customers.TransportTypes;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using Customers.TransportTypes.Attributes;

namespace ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : Controller, ICustomersService
{
    readonly ICustomersService _customersService;
    public CustomersController(ICustomersService customersService)
    {
        _customersService = customersService;
    }

    [HttpPost, Route("register")]
    public async Task<Result<bool>> Register(RegisterCustomer customer)
    {
        return await _customersService.Register(customer);
    }

    [HttpGet, Route("delete"), AuthorizeClient]
    public async Task<Result<bool>> Delete(Guid id)
    {
        return await _customersService.Delete(id);
    }

    [HttpGet, Route("select_by_id"), AuthorizeClient]
    public async Task<Result<Customer?>> SelectById(Guid id)
    {
        return await _customersService.SelectById(id);
    }

    [HttpGet, Route("select_all"), AuthorizeClient]
    public async Task<Result<IEnumerable<Customer>>> SelectAll()
    {
        return await _customersService.SelectAll();
    }

    [HttpPost, Route("login")]
    public async Task<Result<ClientTokenModel>> Login(LoginCustomer customer)
    {
        return await _customersService.Login(customer);
    }

    [HttpGet, Route("waste"), AuthorizeClient]
    public async Task<Result<bool>> Waste(double sum)
    {
        return await _customersService.Waste(sum);
    }

    [HttpGet, Route("earn"), AuthorizeClient]
    public async Task<Result<bool>> Earn(double sum)
    {
        return await _customersService.Earn(sum);
    }
}

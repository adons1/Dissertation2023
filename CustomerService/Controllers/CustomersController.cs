using Core;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using OauthAuthorization.TransportTypes.Attributes;
using OauthAuthorization.TransportTypes.TransportModels;

namespace CustomersService.Controller;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase, ICustomersService
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

    [HttpPost, Route("delete")]
    public async Task<Result<bool>> Delete(Guid id)
    {
        return await _customersService.Delete(id);
    }

    [HttpGet, Route("select_by_id"), Authorize]
    public async Task<Result<Customer?>> SelectById(Guid id)
    {
        return await _customersService.SelectById(id);
    }

    [HttpGet, Route("select_all"), Authorize]
    public async Task<Result<IEnumerable<Customer>>> SelectAll()
    {
        return await _customersService.SelectAll();
    }

    [HttpPost, Route("login")]
    public async Task<Result<ClientTokenModel>> Login(LoginCustomer customer)
    {
        return await _customersService.Login(customer);
    }
}

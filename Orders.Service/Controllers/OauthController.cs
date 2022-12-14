using Core;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OauthAuthorization.TransportTypes.Attributes;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;

namespace Orders.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OauthController : ControllerBase, IOauthServiceAuthorize
{
    readonly IOauthServiceAuthorize _customersService;
    public OauthController(IOauthServiceAuthorize customersService)
    {
        _customersService = customersService;
    }
    [HttpGet, Route("token")]
    public async Task<Result<TokenModel>> Token(string code)
    {
        return await _customersService.Token(code);
    }
}

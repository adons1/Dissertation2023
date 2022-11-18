using Core;
using Microsoft.AspNetCore.Mvc;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;

namespace Products.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OauthController : Controller, IOauthServiceAuthorize
{
    readonly IOauthServiceAuthorize _productsService;
    public OauthController(IOauthServiceAuthorize productsService)
    {
        _productsService = productsService;
    }
    [HttpGet, Route("token")]
    public async Task<Result<TokenModel>> Token(string code)
    {
        return await _productsService.Token(code);
    }
}

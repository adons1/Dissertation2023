using Core;
using Microsoft.AspNetCore.Mvc;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using ServiceProvider = OauthAuthorizationService.Providers.ServiceProvider;
namespace OauthAuthorizationService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OauthController : Controller, IOauthService
{
    readonly IOauthService _oauthService;
    public OauthController(IOauthService oauthService)
    {
        _oauthService = oauthService;
    }
    [HttpGet, Route("authorize")]
    public async Task<Result<AuthCodeModel>> Authorize(Guid issuerId, Guid accepterId, string password)
    {
        return await _oauthService.Authorize(issuerId, accepterId, password);
    }

    [HttpGet, Route("token")]
    public async Task<Result<TokenModel>> Token(string code)
    {
        return await _oauthService.Token(code);
    }

    [HttpGet, Route("verify")]
    public async Task<Result<VerifyModel>> Verify(Guid issuerId, Guid accepterId, string token)
    {
        return await _oauthService.Verify(issuerId, accepterId, token);
    }
}

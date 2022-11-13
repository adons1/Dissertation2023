using Core;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace OauthAuthorization.TransportTypes.TransportServices;

public class OauthHttpService : HttpServiceBase, IOauthService
{
    public OauthHttpService(IConfiguration configuration)
    {
        _baseUrl = configuration["Credentials:AuthorizationService:url"].ToString();
    }
    public async Task<Result<AuthCodeModel>?> Authorize(Guid issuerId, Guid accepterId, string password)
    {
        return await GetAsync<AuthCodeModel>($"/oauth/authorize", new { issuerId, accepterId, password });
    }

    public async Task<Result<TokenModel>?> Token(string code)
    {
        return await GetAsync<TokenModel>($"/oauth/token", new { code });
    }

    public async Task<Result<VerifyModel>?> Verify(Guid issuerId, Guid accepterId, string token)
    {
        return await GetAsync<VerifyModel>($"/oauth/verify", new { issuerId, accepterId, token });
    }
}

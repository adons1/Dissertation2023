using Core;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.TransportServices;

public class OauthHttpService : HttpServiceBase, IOauthService
{
    public OauthHttpService(string baseUrl)
    {
        _baseUrl = baseUrl;
    }
    public async Task<Result<AuthCodeModel>?> Authorize(Guid issuerId, Guid accepterId, string password)
    {
        return await GetAsync<AuthCodeModel>($"/oauth/authorize", new { issuerId, accepterId, password });
    }

    public async Task<Result<TokenModel>?> Token(Guid issuerId, Guid accepterId, string code)
    {
        return await GetAsync<TokenModel>($"/oauth/token", new { issuerId, accepterId, code });
    }

    public async Task<Result<VerifyModel>?> Verify(Guid issuerId, Guid accepterId, string token)
    {
        return await GetAsync<VerifyModel>($"/oauth/verify", new { issuerId, accepterId, token });
    }
}

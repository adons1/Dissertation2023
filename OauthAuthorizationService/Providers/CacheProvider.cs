using Core.RedisKeys;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using OauthAuthorizationService.Models;
using System;
using OauthAuthorization.TransportTypes.TransportModels;

namespace OauthAuthorizationService.Providers;

public class CacheProvider
{
    readonly IDistributedCache _distributedCache;
    public CacheProvider(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    #region AuthCode
    public async Task SetCodeAsync(Guid issuerId, Guid accepterId, DateTime issueDate, string code)
    {
        var authCode = new AuthCodeModel
        {
            IssuerId = issuerId,
            AccepterId = accepterId,
            IssueDate = issueDate,
            Code = code
        };

        await _distributedCache.SetStringAsync(CacheKeys.ServiceCode(code), JsonConvert.SerializeObject(authCode));
    }
    public async Task<AuthCodeModel?> TryGetCodeAsync(string code)
    {
        var authCodeJson = await _distributedCache.GetStringAsync(CacheKeys.ServiceCode(code));
        if (authCodeJson == null) return null;

        var authCode = JsonConvert.DeserializeObject<AuthCodeModel>(authCodeJson);
        if (authCode == null) return null;

        return authCode;
    }

    public async Task RemoveCodeAsync(string code)
    {
        await _distributedCache.RemoveAsync(CacheKeys.ServiceCode(code));  
    }
    #endregion

    #region Token
    public async Task SetTokenAsync(Guid issuerId, Guid accepterId, DateTime issueDate, string token)
    {
        var tokenModel = new TokenModel
        {
            IssuerId = issuerId,
            AccepterId = accepterId,
            IssueDate = issueDate,
            Token = token
        };

        await _distributedCache.SetStringAsync(CacheKeys.ServiceToken(issuerId, accepterId), JsonConvert.SerializeObject(tokenModel));
    }
    public async Task<TokenModel?> TryGetTokenAsync(Guid issuerId, Guid accepterId)
    {
        var tokenJson = await _distributedCache.GetStringAsync(CacheKeys.ServiceToken(issuerId, accepterId));
        if (tokenJson == null) return null;

        var tokenModel = JsonConvert.DeserializeObject<TokenModel>(tokenJson);
        if (tokenModel == null) return null;

        return tokenModel;
    }

    public async Task RemoveTokenAsync(Guid issuerId, Guid accepterId)
    {
        await _distributedCache.RemoveAsync(CacheKeys.ServiceToken(issuerId, accepterId));
    }
    #endregion
}

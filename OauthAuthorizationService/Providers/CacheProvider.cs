using Core.RedisKeys;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using OauthAuthorizationService.Models;
using System;

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

        await _distributedCache.SetStringAsync(ChacheKeys.ServiceCode(issuerId, accepterId), JsonConvert.SerializeObject(authCode));
    }
    public async Task<AuthCodeModel?> TryGetCodeAsync(Guid issuerId, Guid accepterId)
    {
        var authCodeJson = await _distributedCache.GetStringAsync(ChacheKeys.ServiceCode(issuerId, accepterId));
        if (authCodeJson == null) return null;

        var authCode = JsonConvert.DeserializeObject<AuthCodeModel>(authCodeJson);
        if (authCode == null) return null;

        return authCode;
    }

    public async Task RemoveCodeAsync(Guid issuerId, Guid accepterId)
    {
        await _distributedCache.RemoveAsync(ChacheKeys.ServiceCode(issuerId, accepterId));  
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

        await _distributedCache.SetStringAsync(ChacheKeys.ServiceToken(issuerId, accepterId), JsonConvert.SerializeObject(tokenModel));
    }
    public async Task<TokenModel?> TryGetTokenAsync(Guid issuerId, Guid accepterId)
    {
        var tokenJson = await _distributedCache.GetStringAsync(ChacheKeys.ServiceToken(issuerId, accepterId));
        if (tokenJson == null) return null;

        var tokenModel = JsonConvert.DeserializeObject<TokenModel>(tokenJson);
        if (tokenModel == null) return null;

        return tokenModel;
    }

    public async Task RemoveTokenAsync(Guid issuerId, Guid accepterId)
    {
        await _distributedCache.RemoveAsync(ChacheKeys.ServiceToken(issuerId, accepterId));
    }
    #endregion
}

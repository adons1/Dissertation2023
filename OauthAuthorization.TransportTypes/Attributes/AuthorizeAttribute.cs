using authAuthorization.TransportTypes.TransportModels.Enums;
using Core;
using Core.RedisKeys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OauthAuthorization.TransportTypes.TransportModels;

namespace OauthAuthorization.TransportTypes.Attributes;

public class AuthorizeServiceAttribute : Attribute, IAuthorizationFilter
{
    AuthorizationFilterContext _context;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _context = context;
        try
        {
            var service = authorizeService();

            if (!service)
            {
                failureResponse("Token is not specified.");
                return;
            }
        }
        catch (Exception ex)
        {
            failureResponse("Unauthorize.");
            return;
        }
    }

    bool authorizeService()
    {
        var token = _context.HttpContext.Request.Headers["Authorize-Service"].ToString();

        if (string.IsNullOrEmpty(token)) return false;
        var distributedCache = _context.HttpContext.RequestServices.GetService<IDistributedCache>();

        var tokenModelJson = distributedCache.GetStringAsync(CacheKeys.ServiceTokenByToken(token)).Result;
        if (string.IsNullOrEmpty(tokenModelJson)) return false;

        var tokenModel = JsonConvert.DeserializeObject<TokenModel>(tokenModelJson);
        if (DateTime.UtcNow - tokenModel.IssueDate > new TimeSpan(1, 0, 0))
        {
            distributedCache.RemoveAsync(CacheKeys.ServiceTokenByToken(token));
            return false;
        }

        return true;
    }

    private void failureResponse(string message)
    {
        var responseModel = new FailureResult<bool>(message);

        var result = new JsonResult(responseModel);
        result.StatusCode = 401;

        _context.Result = result;
    }
}

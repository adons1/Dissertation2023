using authAuthorization.TransportTypes.TransportModels.Enums;
using Core;
using Core.RedisKeys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OauthAuthorization.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.Attributes;

public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    Authenticate _authenticate;
    public AuthorizeAttribute(Authenticate authenticate = Authenticate.Both)
    {
        _authenticate = authenticate;
    }

    AuthorizationFilterContext _context;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _context = context;
        try
        {
            var client = authorizeClient();
            var service = authorizeService();
            var hasAccess = _authenticate switch
            {
                Authenticate.Both => client || service,
                Authenticate.Client => client,
                Authenticate.Service => service,
                _ => service
            };

            if (!hasAccess)
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

    bool authorizeClient()
    {
        var token = _context.HttpContext.Request.Headers["Authorize-Client"].ToString();
        if (string.IsNullOrEmpty(token)) return false;

        var distributedCache = _context.HttpContext.RequestServices.GetService<IDistributedCache>();

        var tokenModelJson = distributedCache.GetStringAsync(CacheKeys.ClientToken(token)).Result;
        if (string.IsNullOrEmpty(tokenModelJson)) return false;

        var tokenModel = JsonConvert.DeserializeObject<ClientTokenModel>(tokenModelJson);
        if (DateTime.UtcNow - tokenModel.IssueDate > new TimeSpan(1, 0, 0)) return false;

        return true;
    }

    bool authorizeService()
    {
        var token = _context.HttpContext.Request.Headers["Authorize-Service"].ToString();

        if (string.IsNullOrEmpty(token)) return false;
        var distributedCache = _context.HttpContext.RequestServices.GetService<IDistributedCache>();

        var tokenModelJson = distributedCache.GetStringAsync(CacheKeys.ServiceTokenByToken(token)).Result;
        if (string.IsNullOrEmpty(tokenModelJson)) return false;

        var tokenModel = JsonConvert.DeserializeObject<TokenModel>(tokenModelJson);
        if (DateTime.UtcNow - tokenModel.IssueDate > new TimeSpan(1, 0, 0))return false;

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

using authAuthorization.TransportTypes.TransportModels.Enums;
using Core;
using Core.RedisKeys;
using Customers.TransportTypes.Tokens;
using Customers.TransportTypes.TransportModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OauthAuthorization.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.TransportTypes.Attributes;

public class AuthorizeClient : Attribute, IAuthorizationFilter
{
    AuthorizationFilterContext _context;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _context = context;
        try
        {
            var token = _context.HttpContext.Request.Headers["Authorize-Client"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                failureResponse("Unauthorize.");
                return;
            }

            var configuration = _context.HttpContext.RequestServices.GetService<IConfiguration>();
            var distributedCache = _context.HttpContext.RequestServices.GetService<IClientTokensCache>();

            var jwt = new Jwt(token);
            var secret = distributedCache.GetStringAsync(CacheKeys.ClientToken(jwt.Payload.id)).Result;

            if (!jwt.Check(secret))
            {
                failureResponse("Signature is not valid.");
                return;
            }
            if (secret == null)
            {
                failureResponse("Token is expired.");
                return;
            }

            if (DateTime.UtcNow - jwt.Payload.date_issued > new TimeSpan(1, 0, 0))
            {
                distributedCache.RemoveAsync(CacheKeys.ClientToken(jwt.Payload.id));
                failureResponse("Token is expired.");
                return;
            }
        }
        catch (Exception ex)
        {
            failureResponse("Unauthorize.");
            return;
        }
    }

    private void failureResponse(string message)
    {
        var responseModel = new FailureResult<bool>(message);

        var result = new JsonResult(responseModel);
        result.StatusCode = 401;

        _context.Result = result;
    }
}

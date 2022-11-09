using Core;
using Core.RedisKeys;
using CustomersService.TransportTypes.TransportModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Attributes;

public class AuthorizeClientAttribute : Attribute, IAuthorizationFilter
{
    AuthorizationFilterContext _context;
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _context = context;

        var token = context.HttpContext.Request.Headers["Authorize-Client"].ToString();

        if (string.IsNullOrEmpty(token))
        {
            failureResponse("Token is not specified.");
            return;
        }
        var distributedCache = context.HttpContext.RequestServices.GetService<IDistributedCache>();

        var tokenModelJson = distributedCache.GetStringAsync(ChacheKeys.ClientToken(token)).Result;
        if (string.IsNullOrEmpty(tokenModelJson))
        {
            failureResponse("Unknown token.");
            return;
        }

        var tokenModel = JsonConvert.DeserializeObject<TokenModel>(tokenModelJson);
        if (DateTime.UtcNow - tokenModel.IssueDate > new TimeSpan(1, 0, 0))
        {
            failureResponse("Token is expired.");
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

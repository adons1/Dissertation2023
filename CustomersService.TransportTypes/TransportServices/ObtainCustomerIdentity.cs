using Customers.TransportTypes.TransportModels;
using Customers.TransportTypes.TransportServices.Contracts;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Customers.TransportTypes.TransportServices;

public class ObtainCustomerIdentity : IObtainCustomerIdentity
{
    readonly IHttpContextAccessor _contextAccessor;
    public ObtainCustomerIdentity(IHttpContextAccessor contextAccessor, IServiceProvider provider)
    {
        _contextAccessor = contextAccessor;
    }
    public async Task<Guid> Identity()
    {
        var token = _contextAccessor.HttpContext.Request.Headers["Authorize-Client"].ToString();

        if (string.IsNullOrEmpty(token)) throw new SecurityException("User is not authorized");

        var jwt = new Jwt(token);

        if (jwt == null) throw new SecurityException("User is not authorized");

        if (jwt.Payload == null) throw new SecurityException("User is not authorized");

        return jwt.Payload.id;
    }
}

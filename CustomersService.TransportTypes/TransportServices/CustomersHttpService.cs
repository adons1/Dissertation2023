using Core;
using Core.Services;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OauthAuthorization.TransportTypes.Http;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersService.TransportTypes.TransportServices;

public class CustomersHttpService : HttpServiceBase, ICustomersService, IOauthServiceAuthorize
{
    public CustomersHttpService(IDistributedCache distributedCache, IConfiguration configuration)
    {
        _baseUrl = configuration["Credentials:CustomersService:url"].ToString();

        AuthorizedHttp = new OauthHttp(
            disctibutedCache:   distributedCache,
            configuration:      configuration,
            baseUrl:            _baseUrl,
            issuerId:           Guid.Parse(configuration["Credentials:Own:id"].ToString()),
            accepterId:         Guid.Parse(configuration["Credentials:CustomersService:id"].ToString()),
            password:           configuration["Credentials:CustomersService:secret"].ToString()
            );
    }

    public async Task<Result<bool>?> Register(RegisterCustomer customer)
    {
        return await PostAuthorizedAsync<bool>($"/customers/register", body:customer);
    }

    public Task<Result<bool>> Delete(Guid guid)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Customer>>?> SelectAll()
    {
        return await GetAuthorizedAsync<IEnumerable<Customer>>($"/customers/select_all");
    }

    public async Task<Result<Customer>?> SelectById(Guid guid)
    {
        return await GetAuthorizedAsync<Customer?>($"/customers/select_by_id", new { id = guid });
    }

    public Task<Result<ClientTokenModel>> Login(LoginCustomer customer)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TokenModel>> Token(string code)
    {
        throw new NotImplementedException();
    }
}

using Core;
using Core.Services;
using Customers.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public CustomersHttpService(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConfiguration configuration)
    {
        _baseUrl = configuration["Credentials:CustomersService:url"].ToString();

        AuthorizedHttp = new OauthHttp(
            httpContextAccessor: httpContextAccessor,
            disctibutedCache:   distributedCache,
            configuration:      configuration,
            baseUrl:            _baseUrl,
            issuerId:           Guid.Parse(configuration["Credentials:Own:id"].ToString()),
            accepterId:         Guid.Parse(configuration["Credentials:CustomersService:id"].ToString()),
            password:           configuration["Credentials:Own:secret"].ToString()
            );
    }

    public async Task<Result<bool>?> Register(RegisterCustomer customer)
    {
        return await PostAuthorizedAsync<bool>($"/customers/register", body:customer);
    }

    public async Task<Result<bool>> Delete(Guid guid)
    {
        return await GetAuthorizedAsync<bool>($"/customers/delete", new { id = guid });
    }

    public async Task<Result<IEnumerable<Customer>>?> SelectAll()
    {
        return await GetAuthorizedAsync<IEnumerable<Customer>>($"/customers/select_all");
    }

    public async Task<Result<Customer>?> SelectById(Guid guid)
    {
        return await GetAuthorizedAsync<Customer?>($"/customers/select_by_id", new { id = guid });
    }

    public async Task<Result<ClientTokenModel>> Login(LoginCustomer customer)
    {
        return await PostAuthorizedAsync<ClientTokenModel?>($"/customers/login", body: customer);
    }

    public async Task<Result<TokenModel>> Token(string code)
    {
        return await GetAuthorizedAsync<TokenModel?>($"/customers/delete", new { code });
    }

    public async Task<Result<bool>> Waste(IEnumerable<Guid> productIds, double sum)
    {
        return await PostAuthorizedAsync<bool>($"/customers/waste", query:new { sum }, body: productIds);
    }

    public async Task<Result<bool>> Earn(double sum)
    {
        return await GetAuthorizedAsync<bool>($"/customers/earn", new { sum });
    }


}

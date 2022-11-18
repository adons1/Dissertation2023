using Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OauthAuthorization.TransportTypes.Http;
using Products.TransportTypes.TransportModels;
using Products.TransportTypes.TransportServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.TransportTypes.TransportServices;

public class ProductsHttpService : HttpServiceBase, IProductsService
{
    public ProductsHttpService(IDistributedCache distributedCache, IConfiguration configuration)
    {
        _baseUrl = configuration["Credentials:ProductsService:url"].ToString();

        AuthorizedHttp = new OauthHttp(
            disctibutedCache: distributedCache,
            configuration: configuration,
            baseUrl: _baseUrl,
            issuerId: Guid.Parse(configuration["Credentials:Own:id"].ToString()),
            accepterId: Guid.Parse(configuration["Credentials:ProductsService:id"].ToString()),
            password: configuration["Credentials:ProductsService:secret"].ToString()
            );
    }

    public Task<Result<bool>> Delete()
    {
        throw new NotImplementedException();
    }

    public Task<Result<Product>> Select()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Product>>> SelectAll()
    {
        return await GetAuthorizedAsync<IEnumerable<Product>>($"/products/select_all");
    }

    public Task<Result<bool>> Update()
    {
        throw new NotImplementedException();
    }
}

using Core;
using Microsoft.AspNetCore.Http;
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
    public ProductsHttpService(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConfiguration configuration)
    {
        _baseUrl = configuration["Credentials:ProductsService:url"].ToString();

        AuthorizedHttp = new OauthHttp(
            httpContextAccessor: httpContextAccessor,
            disctibutedCache:   distributedCache,
            configuration:      configuration,
            baseUrl:            _baseUrl,
            issuerId:           Guid.Parse(configuration["Credentials:Own:id"].ToString()),
            accepterId:         Guid.Parse(configuration["Credentials:ProductsService:id"].ToString()),
            password:           configuration["Credentials:Own:secret"].ToString()
            );
    }

    public async Task<Result<ConsumeProductsResult>> ConsumeProducts(Guid productId, int quantity)
    {
        return await GetAuthorizedAsync<ConsumeProductsResult>($"/products/consume", new { productId, quantity });
    }

    public async Task<Result<bool>> Delete(Guid id)
    {
        return await GetAuthorizedAsync<bool>($"/products/delete");
    }

    public async Task<Result<Product>> Select(Guid id)
    {
        return await GetAuthorizedAsync<Product>($"/products/select_by_id");
    }

    public async Task<Result<IEnumerable<Product>>> SelectAll()
    {
        return await GetAuthorizedAsync<IEnumerable<Product>>($"/products/select_all");
    }

    public async Task<Result<IEnumerable<Product>>> SelectByIds(IEnumerable<Guid> ids)
    {
        return await PostAuthorizedAsync<IEnumerable<Product>>($"/products/select_by_ids", body: ids);
    }

    public async Task<Result<bool>> SupplyProducts(Guid id, int quantity)
    {
        return await GetAuthorizedAsync<bool>($"/products/supply");
    }

    public async Task<Result<bool>> Update(Product product)
    {
        return await GetAuthorizedAsync<bool>($"/products/update");
    }
}

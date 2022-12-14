using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OauthAuthorization.TransportTypes.Http;
using Orders.Service.TransportTypes.TransportServices.Contracts;
using Orders.TransportTypes.TransportModels;
using Products.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.TransportTypes.TransportServices
{
    public class OrdersHttpService : HttpServiceBase, IOrdersService
    {
        public OrdersHttpService(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _baseUrl = configuration["Credentials:OrdersService:url"].ToString();

            AuthorizedHttp = new OauthHttp(
                httpContextAccessor: httpContextAccessor,
                disctibutedCache: distributedCache,
                configuration: configuration,
                baseUrl: _baseUrl,
                issuerId: Guid.Parse(configuration["Credentials:Own:id"].ToString()),
                accepterId: Guid.Parse(configuration["Credentials:OrdersService:id"].ToString()),
                password: configuration["Credentials:Own:secret"].ToString()
                );
        }
        public async Task<Result<bool>> Cancel(Guid id)
        {
            return await GetAuthorizedAsync<bool>($"/orders/cancel", new { id });
        }

        public async Task<Result<Order>> Create(IEnumerable<Guid> productsId)
        {
            return await PostAuthorizedAsync<Order>($"/orders/create", body: productsId);
        }

        public async Task<Result<IEnumerable<Order>>> SelectAll()
        {
            return await GetAuthorizedAsync<IEnumerable<Order>>($"/orders/select_all");
        }

        public async Task<Result<Order>> SelectById(Guid id)
        {
            return await GetAuthorizedAsync<Order>($"/orders/select_by_id", new { id });
        }
    }
}

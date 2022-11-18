using Core;
using Core.RedisKeys;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using Products.Service.Providers.ProvidersContracts;
using Products.TransportTypes.TransportModels;
using Products.TransportTypes.TransportServices.Contracts;

namespace Products.Service.Services;

public class ProductsService : IProductsService, IOauthServiceAuthorize
{
    readonly IProductsProvider _productsProvider;
    readonly IOauthService _oauthService;
    readonly IDistributedCache _distributedCache;
    readonly IConfiguration _configuration;
    public ProductsService(
        IProductsProvider productsProvider,
        IOauthService oauthService,
        IDistributedCache distributedCache,
        IConfiguration configuration)
    {
        _productsProvider = productsProvider;
        _oauthService = oauthService;
        _distributedCache = distributedCache;
        _configuration = configuration;
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
        var products = new SuccessResult<IEnumerable<Product>>(_productsProvider.SelectAll());
        return await Task.FromResult(products);
    }

    public async Task<Result<TokenModel>> Token(string code)
    {
        var token = await _oauthService.Token(code);

        _distributedCache.SetStringAsync(CacheKeys.ServiceTokenByToken(token.Payload.Token), JsonConvert.SerializeObject(token.Payload));

        return token;
    }

    public Task<Result<bool>> Update()
    {
        throw new NotImplementedException();
    }
}

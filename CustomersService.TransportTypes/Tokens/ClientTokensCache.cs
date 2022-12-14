using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.TransportTypes.Tokens;

public class ClientTokensCacheOptions : RedisCacheOptions
{
}

public class ClientTokensCache : RedisCache, IClientTokensCache
{
    public ClientTokensCache(IOptions<ClientTokensCacheOptions> optionsAccessor) : base(optionsAccessor)
    {
    }
}

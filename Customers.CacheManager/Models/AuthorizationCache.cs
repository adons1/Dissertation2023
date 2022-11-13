using Customers.CacheManager.Models.CacheInterfaces;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.CacheManager.Models;

internal class AuthorizationCacheOptions : RedisCacheOptions
{
}

internal class AuthorizationCache : RedisCache, IAuthorizationCache
{
    public AuthorizationCache(IOptions<AuthorizationCacheOptions> optionsAccessor) : base(optionsAccessor)
    {
    }
}


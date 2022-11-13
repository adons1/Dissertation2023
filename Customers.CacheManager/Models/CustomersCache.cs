using Customers.CacheManager.Models.CacheInterfaces;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.CacheManager.Models;
internal class CustomersCacheOptions : RedisCacheOptions
{
}

internal class CustomersCache : RedisCache, ICustomersCache
{
    public CustomersCache(IOptions<CustomersCacheOptions> optionsAccessor) : base(optionsAccessor)
    {
    }
}

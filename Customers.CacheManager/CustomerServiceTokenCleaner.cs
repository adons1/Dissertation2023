using Core.RedisKeys;
using CustomersService.TransportTypes.TransportModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Customers.CacheManager
{
    public class CustomerServiceTokenCleaner : BackgroundService
    {
        private readonly ILogger<CustomerServiceTokenCleaner> _logger;
        private readonly IServer _server;
        private readonly IDistributedCache _distributedCache;

        public CustomerServiceTokenCleaner(ILogger<CustomerServiceTokenCleaner> logger, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _logger = logger;

            var redisConnection = configuration.GetConnectionString("RedisConnection");
            var options = ConfigurationOptions.Parse(redisConnection);
            var connection = ConnectionMultiplexer.Connect(options);
            _distributedCache = distributedCache;
            var endPoint = connection.GetEndPoints().First();
            _server = connection.GetServer(endPoint);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var dictionary = new Dictionary<string, TokenModel>();
                var keys = _server.Keys(pattern: "*customer_token*").ToArray();
                foreach (var key in keys)
                {
                    var tokenJson = await _distributedCache.GetStringAsync(key);

                    var token = JsonConvert.DeserializeObject<TokenModel>(tokenJson);
                    if (DateTime.UtcNow - token.IssueDate > new TimeSpan(1, 0, 0))
                        await _distributedCache.RemoveAsync(key);

                    dictionary.Add(token.Token, token);
                }

                var tokensToRemove = (from kv in dictionary.GroupBy(x => x.Value.ClientId, x => x.Value)
                                      let notLatestTokens = kv.Where(v => v.IssueDate != kv.Max(x => x.IssueDate))
                                      select notLatestTokens).SelectMany(x => x).ToList();

                tokensToRemove.ForEach(x => _distributedCache.RemoveAsync(ChacheKeys.ClientToken(x.Token)));

                dictionary.Clear();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
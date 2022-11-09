using Customers.CacheManager;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var redisConnection = ctx.Configuration.GetConnectionString("RedisConnection");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
        });
        services.AddHostedService<CustomerServiceTokenCleaner>();
    })
    .Build();

host.Run();

using Core.CacheManager.Cleaners;
using Customers.CacheManager.Models;
using Customers.CacheManager.Models.CacheInterfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<CustomersCacheOptions>(config =>
        {
            config.Configuration = ctx.Configuration.GetConnectionString("CustomersRedisConnection");
            config.InstanceName = "CustomersCache";
        });
        services.Configure<AuthorizationCacheOptions>(config =>
        {
            config.Configuration = ctx.Configuration.GetConnectionString("AuthorizationRedisConnection");
            config.InstanceName = "AuthorizationCache";
        });

        services.Add(ServiceDescriptor.Singleton<ICustomersCache, CustomersCache>());
        services.Add(ServiceDescriptor.Singleton<IAuthorizationCache, AuthorizationCache>());

        services.AddHostedService<CustomerServiceTokenCleaner>();
        services.AddHostedService<AuthorizationServiceTokenCleaner>();
    })
    .Build();

host.Run();

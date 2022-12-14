using CustomersService.TransportTypes.TransportServices;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Orders.Service.TransportTypes.TransportServices.Contracts;
using Orders.Service.TransportTypes.TransportServices;
using Products.TransportTypes.TransportServices;
using Products.TransportTypes.TransportServices.Contracts;
using Orders.TransportTypes.TransportServices;
using Customers.TransportTypes.Tokens;

var builder = WebApplication.CreateBuilder(args);
string redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
string authCacheConnection = builder.Configuration.GetConnectionString("AuthorizationCache");

builder.Services.Configure<ClientTokensCacheOptions>(config =>
{
    config.Configuration = authCacheConnection;
    config.InstanceName = "AuthorizationCache";
});
builder.Services.Add(ServiceDescriptor.Singleton<IClientTokensCache, ClientTokensCache>());

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ICustomersService, CustomersHttpService>();
builder.Services.AddTransient<IOrdersService, OrdersHttpService>();
builder.Services.AddTransient<IProductsService, ProductsHttpService>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();
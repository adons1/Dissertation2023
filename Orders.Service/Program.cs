
using CustomersService.TransportTypes.TransportServices;
using CustomersService.TransportTypes.TransportServices.Contracts;
using OauthAuthorization.TransportTypes.TransportServices;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Products.TransportTypes.TransportServices.Contracts;
using Products.TransportTypes.TransportServices;
using Microsoft.EntityFrameworkCore;
using Orders.Service.Data;
using Orders.Service.TransportTypes.TransportServices.Contracts;
using Orders.Service.Services;
using Customers.TransportTypes.TransportServices.Contracts;
using Customers.TransportTypes.TransportServices;
using Orders.Service.Provider.ProviderContracts;
using Orders.Service.Provider;
using Orders.Service;
using AutoMapper;
using Customers.TransportTypes.Tokens;

var builder = WebApplication.CreateBuilder(args);
string dbConnection = builder.Configuration.GetConnectionString("DatabaseConnection");
string redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
string authCacheConnection = builder.Configuration.GetConnectionString("AuthorizationCache");

builder.Services.Configure<ClientTokensCacheOptions>(config =>
{
    config.Configuration = authCacheConnection;
    config.InstanceName = "AuthorizationCache";
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddDbContext<OrdersDbContext>(options => options.UseSqlServer(dbConnection));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ICustomersService, CustomersHttpService>();
builder.Services.AddTransient<IOauthServiceAuthorize, OrdersService>();
builder.Services.AddTransient<IProductsService, ProductsHttpService>();
builder.Services.AddTransient<IOauthService, OauthHttpService>();
builder.Services.AddTransient<IOrdersService, OrdersService>();
builder.Services.AddTransient<IOrdersProvider, OrdersProvider>();
builder.Services.AddTransient<IObtainCustomerIdentity, ObtainCustomerIdentity>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();

using Microsoft.EntityFrameworkCore;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using OauthAuthorization.TransportTypes.TransportServices;
using Products.Service.Data;
using Products.Service.Providers.ProvidersContracts;
using Products.TransportTypes.TransportServices.Contracts;
using Products.Service.Services;
using Products.Service.Providers;
using Products.Service;
using Customers.TransportTypes.TransportServices.Contracts;
using Customers.TransportTypes.TransportServices;
using CustomersService.TransportTypes.TransportServices.Contracts;
using CustomersService.TransportTypes.TransportServices;
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
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddDbContext<ProductsDbContext>(options => options.UseSqlServer(dbConnection));
builder.Services.AddTransient<IProductsProvider, ProductsProvider>();
builder.Services.AddTransient<IProductsService, ProductsService>();
builder.Services.AddTransient<IObtainCustomerIdentity, ObtainCustomerIdentity>();
builder.Services.AddTransient<ICustomersService, CustomersHttpService>();
builder.Services.AddTransient<IOauthServiceAuthorize, ProductsService>();
builder.Services.AddTransient<IOauthService, OauthHttpService>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();
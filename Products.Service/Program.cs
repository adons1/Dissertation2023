using Microsoft.EntityFrameworkCore;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using OauthAuthorization.TransportTypes.TransportServices;
using Products.Service.Data;
using Products.Service.Providers.ProvidersContracts;
using Products.TransportTypes.TransportServices.Contracts;
using Products.Service.Services;
using Products.Service.Providers;
using Products.Service;

var builder = WebApplication.CreateBuilder(args);
string dbConnection = builder.Configuration.GetConnectionString("DatabaseConnection");
string redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddDbContext<ProductsDbContext>(options => options.UseSqlServer(dbConnection));
builder.Services.AddTransient<IProductsProvider, ProductsProvider>();
builder.Services.AddTransient<IProductsService, ProductsService>();
builder.Services.AddTransient<IOauthServiceAuthorize, ProductsService>();
builder.Services.AddTransient<IOauthService, OauthHttpService>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();
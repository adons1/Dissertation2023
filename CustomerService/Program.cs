using CustomersService.Data;
using CustomersService.Providers;
using CustomersService.Providers.ProvidersContracts;
using CustomersService.TransportTypes.TransportServices.Contracts;
using Microsoft.EntityFrameworkCore;
using OauthAuthorization.TransportTypes.TransportServices;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;

var builder = WebApplication.CreateBuilder(args);
string dbConnection = builder.Configuration.GetConnectionString("DatabaseConnection");
string redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});
builder.Services.AddDbContext<CustomersDbContext>(options => options.UseSqlServer(dbConnection)); 
builder.Services.AddTransient<ICustomersProvider, CustomersProvider>();
builder.Services.AddTransient<ICustomersService, CustomersService.Services.CustomersService>();
builder.Services.AddTransient<IOauthServiceAuthorize, CustomersService.Services.CustomersService>();
builder.Services.AddTransient<IOauthService, OauthHttpService>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();

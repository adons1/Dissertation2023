
using CustomersService.TransportTypes.TransportServices;
using CustomersService.TransportTypes.TransportServices.Contracts;
using OauthAuthorization.TransportTypes.TransportServices;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
string redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});
builder.Services.AddTransient<ICustomersService, CustomersHttpService>();
builder.Services.AddTransient<IOauthService, OauthHttpService>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();

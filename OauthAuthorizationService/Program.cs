using OauthAuthorizationService.Models;
using LinqToDB.AspNet;
using OauthAuthorizationService.DataConnections;
using LinqToDB.Configuration;
using ServiceProvider = OauthAuthorizationService.Providers.ServiceProvider;
using OauthAuthorizationService.Services;
using OauthAuthorizationService.Providers;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;

var builder = WebApplication.CreateBuilder(args);
string dbConnection = builder.Configuration.GetConnectionString("DatabaseConnection");
string redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});
builder.Services.AddLinqToDBContext<ServicesDbContext>((provider, options) => {
    options.UseSqlServer(dbConnection);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IOauthService, OauthService>();
builder.Services.AddTransient<CacheProvider>();
builder.Services.AddTransient<ServiceProvider>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();

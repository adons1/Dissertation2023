using LinqToDB.Configuration;
using LinqToDB;
using LinqToDB.Data;
using OauthAuthorizationService.Models;

namespace OauthAuthorizationService.DataConnections;

public class ServicesDbContext : DataConnection
{
    public ServicesDbContext(LinqToDBConnectionOptions<ServicesDbContext> options) : base(options) { }
}

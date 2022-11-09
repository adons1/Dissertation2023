using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Routing.Constraints;
using OauthAuthorizationService.DataConnections;
using OauthAuthorizationService.Models;

namespace OauthAuthorizationService.Providers;

public class ServiceProvider
{
    readonly ServicesDbContext _db;
    public ServiceProvider(ServicesDbContext db){ _db = db; }

    public async Task<bool> TryLogonService(
                Guid? Id,
                string passwordHash)
    {
        var param = new DataParameter[]
        {
                new("@Id", Id, DataType.Guid),
                new("@Password", passwordHash, DataType.NVarChar)
        };

        var s =  (await _db.QueryProcAsync<bool>("dbo.TryLogonService", param)).SingleOrDefault();
        return s;
    } 
}

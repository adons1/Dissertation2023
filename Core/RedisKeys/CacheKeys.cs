using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RedisKeys;

public static class CacheKeys
{
    public static string Customer(Guid id) => $"[customer]{id}";
    public static string Customers() => $"[customer]*";
    public static string ClientToken(Guid id) => $"[customer_token]|{id}";

    public static string ServiceCode(string code) => $"[service_code]|{code}";
    public static string ServiceToken(Guid issuerId, Guid accepterId) => $"[service_token]|{issuerId}|{accepterId}";
    public static string ServiceTokenByToken(string token) => $"[service_token_by_token]|{token}";

    public static string Product(Guid id) => $"[product]{id}";


    public static string Rollback(Guid id) => $"[rollback]{id}";
}

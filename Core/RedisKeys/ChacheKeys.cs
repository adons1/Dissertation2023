using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RedisKeys;

public static class ChacheKeys
{
    public static string Customer(Guid id) => $"[customer]{id}";
    public static string Customers() => $"[customer]*";
    public static string ClientToken(string token) => $"[customer_token]|{token}";

    public static string ServiceCode(Guid issuerId, Guid accepterId) => $"[service_code]|{issuerId}|{accepterId}";
    public static string ServiceToken(Guid issuerId, Guid accepterId) => $"[service_token]|{issuerId}|{accepterId}";
}

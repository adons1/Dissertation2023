using Core.Cryptography;
using CustomersService.TransportTypes.TransportModels;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

namespace Customers.TransportTypes.TransportModels;

public class Jwt
{
    string _jwt;
    public JwtHeader Header { get; set; }
    public JwtPayload Payload { get; set; }
    public string Signature { get; set; }

    public Jwt(Customer customer)
    {
        Header = new JwtHeader();
        Payload = new JwtPayload
        {
            id = customer.Id,
            email = customer.Email,
            date_issued = DateTime.UtcNow,
        };
    }

    public Jwt(string jwt)
    {
        _jwt = jwt;
        var jwtParts = jwt.Split('.');

        Header = JsonConvert.DeserializeObject<JwtHeader>(Encoding.UTF8.GetString(Convert.FromBase64String(jwtParts[0])));
        Payload = JsonConvert.DeserializeObject<JwtPayload>(Encoding.UTF8.GetString(Convert.FromBase64String(jwtParts[1])));
        Signature = jwtParts[2];
    }

    public string Sign(string secret)
    {
        var bytesHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Header)));
        var bytesPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Payload)));

        Signature = SHA256.Hash($"{bytesHeader}.{bytesPayload}.{secret}");

        return $"{bytesHeader}.{bytesPayload}.{Signature}";
    }

    public bool Check(string secret)
    {
        var jwtParts = _jwt.Split('.');

        return jwtParts[2] == SHA256.Hash($"{jwtParts[0]}.{jwtParts[1]}.{secret}");
    }
}

public class JwtHeader
{
    public string alg { get => "HS256"; }
    public string typ { get => "JWT"; }
}

public class JwtPayload
{
    public Guid id { get; set; }
    public string email { get; set; }
    public DateTime date_issued { get; set; }
}

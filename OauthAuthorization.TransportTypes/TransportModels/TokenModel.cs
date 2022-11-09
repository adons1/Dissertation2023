using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.TransportModels;

[JsonObject("payload")]
public class TokenModel
{
    [JsonProperty("expires_in")]
    public long ExpiresIn { get; set; }
    [JsonProperty("token")]
    public string Token { get; set; }
}

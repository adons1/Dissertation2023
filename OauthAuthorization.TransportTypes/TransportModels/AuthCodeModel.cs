using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.TransportModels;

[JsonObject("payload")]
public class AuthCodeModel
{
    [JsonProperty("code")]
    public string Code { get; set; }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.TransportModels;

[JsonObject("payload")]
public class VerifyModel
{
    [JsonProperty("status")]
    public bool Status { get; set; }
}

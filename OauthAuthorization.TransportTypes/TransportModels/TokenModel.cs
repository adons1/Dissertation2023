using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.TransportModels;

public class TokenModel
{
    public Guid IssuerId { get; set; }
    public Guid AccepterId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Token { get; set; }
}

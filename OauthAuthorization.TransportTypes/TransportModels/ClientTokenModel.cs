using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.TransportModels;

public class ClientTokenModel
{
    public Guid ClientId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Token { get; set; }
}

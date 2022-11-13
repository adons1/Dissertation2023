using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authAuthorization.TransportTypes.TransportModels.Enums;

public enum Authenticate
{
    Both = 0,
    Client = 1,
    Service = 2
}
public enum Authorize
{
    Both = 0,
    OnlyClient = 1,
    OnlyService = 2
}

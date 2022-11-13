using Core;
using OauthAuthorization.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.TransportServices.Contracts;

public interface IOauthServiceAuthorize
{
    Task<Result<TokenModel>> Token(string code);
}

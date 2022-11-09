using Core;
using OauthAuthorization.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OauthAuthorization.TransportTypes.TransportServices.Contracts;

public interface IOauthService
{
    Task<Result<AuthCodeModel>> Authorize(Guid issuerId, Guid accepterId, string password);
    Task<Result<TokenModel>> Token(Guid issuerId, Guid accepterId, string code);
    Task<Result<VerifyModel>> Verify(Guid issuerId, Guid accepterId, string token);
}

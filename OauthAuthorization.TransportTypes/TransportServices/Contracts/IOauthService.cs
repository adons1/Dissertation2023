using Core;
using OauthAuthorization.TransportTypes.TransportModels;

namespace OauthAuthorization.TransportTypes.TransportServices.Contracts;

public interface IOauthService
{
    Task<Result<AuthCodeModel>> Authorize(Guid issuerId, Guid accepterId, string password);
    Task<Result<TokenModel>> Token(string code);
    Task<Result<VerifyModel>> Verify(Guid issuerId, Guid accepterId, string token);
}

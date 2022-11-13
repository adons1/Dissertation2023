using OauthAuthorizationService.Providers;
using Core.Cryptography;
using Core;
using System.Net;
using OauthAuthorization.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportServices.Contracts;
using System.Linq.Expressions;

namespace OauthAuthorizationService.Services;

public class OauthService : IOauthService
{
    readonly Providers.ServiceProvider _serviceProvider;
    readonly CacheProvider _cacheProvider;
    public OauthService(Providers.ServiceProvider serviceProvider, CacheProvider cacheProvider)
    {
        _serviceProvider = serviceProvider;
        _cacheProvider = cacheProvider;
    }
    public async Task<Result<AuthCodeModel>> Authorize(Guid issuerId, Guid accepterId, string password)
    {
        try
        {
            var isSuccessful = await _serviceProvider.TryLogonService(issuerId, password);
            if (isSuccessful)
            {
                var timestamp = DateTime.UtcNow;

                var plainText = $"{issuerId}|{accepterId}|{password}|{timestamp.Ticks}";

                var code = SHA256.Hash(plainText);

                await _cacheProvider.SetCodeAsync(issuerId, accepterId, timestamp, code);

                return new SuccessResult<AuthCodeModel>(
                    new() { 
                        Code = code,
                        IssuerId = issuerId,
                        AccepterId = accepterId,
                        IssueDate = timestamp
                    });
            }

            return new FailureResult<AuthCodeModel>("Unauthorized.", HttpStatusCode.Unauthorized);
        }
        catch(Exception ex){
            throw;   
        }
    }

    public async Task<Result<TokenModel>> Token(string code)
    {
        var codeModel = await _cacheProvider.TryGetCodeAsync(code);
        if (codeModel == null)
            return new FailureResult<TokenModel>("Code haven`t been found.");

        if (DateTime.UtcNow - codeModel.IssueDate > new TimeSpan(0, 0, 10))
            return new FailureResult<TokenModel>("Code has expired.");

        var randomizer = new Randomizer();
        var token = randomizer.RandomString(100, true);

        await _cacheProvider.SetTokenAsync(codeModel.IssuerId, codeModel.AccepterId, DateTime.UtcNow, token);
        await _cacheProvider.RemoveCodeAsync(codeModel.Code);

        return new SuccessResult<TokenModel>(
            new() { 
                Token = token, 
                IssuerId = codeModel.IssuerId, 
                AccepterId = codeModel.AccepterId, 
                IssueDate = codeModel.IssueDate 
            });
    }

    public async Task<Result<VerifyModel>> Verify(Guid issuerId, Guid accepterId, string token)
    {
        var tokenModel = await _cacheProvider.TryGetTokenAsync(issuerId, accepterId);
        if (tokenModel == null)
            return new FailureResult<VerifyModel>("Token not found.", HttpStatusCode.Unauthorized, payload: new() { Status = false });

        if (DateTime.UtcNow - tokenModel.IssueDate > new TimeSpan(0, 10, 0))
        {
            await _cacheProvider.RemoveTokenAsync(issuerId, accepterId);
            return new FailureResult<VerifyModel>("Token has expired.", payload: new() { Status = false });
        }

        return new SuccessResult<VerifyModel>(new() { Status = true });
    }
}

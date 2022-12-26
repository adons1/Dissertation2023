using Core;
using Core.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Core.RedisKeys;
using System.Web;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using OauthAuthorization.TransportTypes.TransportModels;
using authAuthorization.TransportTypes.TransportModels.Enums;
using Microsoft.AspNetCore.Http;
using Core.Exceptions;

namespace OauthAuthorization.TransportTypes.Http;

public class OauthHttp : IAuthorizedHttp
{
    protected string _clientToken;
    protected string _authorizationBaseUrl;
    protected string _baseUrl;
    protected Guid _issuerId;
    protected Guid _accepterId;
    protected string _password;

    protected IDistributedCache _disctibutedCache;
    protected IHttpContextAccessor _httpContextAccessor;
    protected IConfiguration _configuration;
    public OauthHttp(IHttpContextAccessor httpContextAccessor, IDistributedCache disctibutedCache, IConfiguration configuration, 
        string baseUrl, Guid issuerId, Guid accepterId, string password)
    {
        _disctibutedCache = disctibutedCache;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;

        _clientToken = httpContextAccessor.HttpContext.Request.Headers["Authorize-Client"].ToString();
        _authorizationBaseUrl = configuration["Credentials:AuthorizationService:url"].ToString();
        _baseUrl = baseUrl;
        _issuerId = issuerId;
        _accepterId = accepterId;
        _password = password;
    }
    public async Task<Result<TResult>> GetAuthorizedAsync<TResult>(string url, object parametres)
    {
        string reqid = "";
        var requestId = _httpContextAccessor.HttpContext.Request.Headers["RequestID"];
        if (string.IsNullOrEmpty(requestId))
        {
            reqid = Guid.NewGuid().ToString();
        }
        else
        {
            reqid = requestId.ToString();
        }

        using (var client = new HttpClient())
        {
            var (hasToken, token) = await getTokenCache();
            if (hasToken)
                client.DefaultRequestHeaders.Add("Authorize-Service", token.Token);

            client.DefaultRequestHeaders.Add("Authorize-Client", _clientToken);
            client.DefaultRequestHeaders.Add("RequestID", reqid);

            var requestUrl = HttpServiceBase.GetUrl(_baseUrl, url, parametres);
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<Result<TResult>>(content);
                if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                    throw new RollbackException(Guid.Parse(reqid), response.StatusCode.ToString());

                try
                {
                    result.Payload = JsonConvert.DeserializeObject<TResult>(JObject.Parse(content)["payload"].ToString());
                }
                catch { }

                return result;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                client.DefaultRequestHeaders.Remove("Authorize-Service");

                var obtainedCode = await Authorize();
                var obtainedToken = await Token(obtainedCode);

                await setTokenCache(obtainedToken);

                return await GetAuthorizedAsync<TResult>(url, parametres);
            }

            throw new RollbackException(Guid.Parse(reqid), response.StatusCode.ToString());
        };
    }

    public async Task<Result<TResult>> PostAuthorizedAsync<TResult>(string url, object? header = null, object? query = null, object? body = null)
    {
        string reqid = "";
        var requestId = _httpContextAccessor.HttpContext.Request.Headers["RequestID"];
        if(string.IsNullOrEmpty(requestId))
        {
            reqid = Guid.NewGuid().ToString();
        }
        else
        {
            reqid = requestId.ToString();
        }

        using (var client = new HttpClient())
        {
            var (hasToken, token) = await getTokenCache();
            if (hasToken)
                client.DefaultRequestHeaders.Add("Authorize-Service", token.Token);

            client.DefaultRequestHeaders.Add("Authorize-Client", _clientToken);
            client.DefaultRequestHeaders.Add("RequestID", reqid);

            var requestUrl = HttpServiceBase.GetUrl(_baseUrl, url, query);

            HttpServiceBase.SetHeader(client, header);

            var jsonContent = JsonContent.Create(body);

            var response = await client.PostAsync(requestUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<Result<TResult>>(content);
                if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                    throw new RollbackException(Guid.Parse(reqid), response.StatusCode.ToString());

                try
                {
                    result.Payload = JsonConvert.DeserializeObject<TResult>(JObject.Parse(content)["payload"].ToString());
                }
                catch { }

                return result;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                client.DefaultRequestHeaders.Remove("Authorize-Service");

                var obtainedCode = await Authorize();
                var obtainedToken = await Token(obtainedCode);

                await setTokenCache(obtainedToken);

                return await PostAuthorizedAsync<TResult>(url, header, query, body);
            }

            throw new RollbackException(Guid.Parse(reqid), response.StatusCode.ToString());
        };
    }
    #region Private
    private async Task<AuthCodeModel> Authorize()
    {
        using(var client = new HttpClient())
        {
            var authUrl = HttpServiceBase.GetUrl(
                    _authorizationBaseUrl,
                    "/oauth/authorize",
                    new
                    {
                        issuerId = _issuerId,
                        accepterId = _accepterId,
                        password = _password
                    });

            var response = await client.GetAsync(authUrl);

            if (!response.IsSuccessStatusCode) throw new Exception(response.StatusCode.ToString());

            var content = await response.Content.ReadAsStringAsync();

            var payload = JsonConvert.DeserializeObject<AuthCodeModel>(JObject.Parse(content)["payload"].ToString());

            return payload;
        }
    }

    private async Task<TokenModel> Token(AuthCodeModel code)
    {
        using (var client = new HttpClient())
        {
            var tokenUrl = HttpServiceBase.GetUrl(
                    _baseUrl,
                    "/oauth/token",
                    new
                    {
                        code = code.Code,
                    });

            var response = await client.GetAsync(tokenUrl);

            if (!response.IsSuccessStatusCode) throw new Exception(response.StatusCode.ToString());

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TokenModel>(JObject.Parse(content)["payload"].ToString());
        } 
    }

    private async Task<(bool, TokenModel)> getTokenCache()
    {
        var tokenJson = await _disctibutedCache.GetStringAsync(CacheKeys.ServiceToken(_issuerId, _accepterId));
        if (string.IsNullOrEmpty(tokenJson) || tokenJson == "null") return (false, null);

        var token = JsonConvert.DeserializeObject<TokenModel>(tokenJson);

        return (true, token);
    }

    private async Task setTokenCache(TokenModel token)
    {
        var tokenJson = JsonConvert.SerializeObject(token);
        await _disctibutedCache.SetStringAsync(CacheKeys.ServiceToken(_issuerId, _accepterId), tokenJson);
    }

    #endregion
}

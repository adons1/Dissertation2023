using Core.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Web;

namespace Core;

public abstract class HttpServiceBase : IAuthorizedHttp
{
    protected string _baseUrl;
    public IAuthorizedHttp AuthorizedHttp;

    protected async Task<Result<TResult>> GetAsync<TResult>(string url, object parametres)
    {
        using(var client = new HttpClient())
        {
            var requestUrl = GetUrl(_baseUrl, url, parametres);
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<Result<TResult>>(content);
                var payload = JsonConvert.DeserializeObject<TResult>(JObject.Parse(content)["payload"].ToString());

                result.Payload = payload;

                return result;
            }

            throw new Exception(response.StatusCode.ToString());
        };
    }
    protected async Task<Result<TResult>> PostAsync<TResult>(string url, object? header = null, object? query = null, object? body = null)
    {
        using (var client = new HttpClient())
        {
            var requestUrl = GetUrl(_baseUrl, url, query);

            SetHeader(client, header);

            var jsonContent = JsonContent.Create(body);

            var response = await client.PostAsync(requestUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<Result<TResult>>(content);
                var payload = JsonConvert.DeserializeObject<TResult>(JObject.Parse(content)["payload"].ToString());

                result.Payload = payload;

                return result;
            }

            throw new Exception(response.StatusCode.ToString());
        };
    }
    public async Task<Result<TResult>> GetAuthorizedAsync<TResult>(string url, object? parametres = null)
    {
        return await AuthorizedHttp.GetAuthorizedAsync<TResult>(url, parametres);
    }
    public async Task<Result<TResult>> PostAuthorizedAsync<TResult>(string url, object? header = null, object? query = null, object? body = null)
    {
        return await AuthorizedHttp.PostAuthorizedAsync<TResult>(url, header, query, body);
    }

    #region Static
    public static string GetUrl(string baseUrl, string url, object? parametres)
    {
        if(parametres == null) return ($"{baseUrl}{url}");

        var dictionary = new Dictionary<string, object>();

        var properties = from p in parametres.GetType().GetProperties()
                         where p.GetValue(parametres, null) != null
                         select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(parametres, null).ToString());

        if (properties.Any())
        {
            string queryString = String.Join("&", properties.ToArray());

            return ($"{baseUrl}{url}?{queryString}");
        }
        else return ($"{baseUrl}{url}");
    }

    public static void SetHeader(HttpClient client, object? headerParams)
    {
        var properties = from p in headerParams.GetType().GetProperties()
                         where p.GetValue(headerParams, null) != null
                         select new KeyValuePair<string, string>(p.Name, p.GetValue(headerParams, null).ToString());

        foreach (var property in properties)
        {
            client.DefaultRequestHeaders.Add(property.Key, property.Value);
        }
    }
    #endregion
}

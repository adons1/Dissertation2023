using Newtonsoft.Json;
using System.Net;

namespace Core;

public class Result<T>
{
    [JsonProperty(PropertyName = "status")]
    public ServiceResult Status { get; protected set; }
    [JsonProperty(PropertyName = "statusCode")]
    public HttpStatusCode StatusCode { get; protected set; }
    public T? Payload { get; set; }
    [JsonProperty(PropertyName = "message")]
    public string Message { get; protected set; }
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T? payload = default(T))
    {
        Status = ServiceResult.Success;
        StatusCode = HttpStatusCode.OK;
        Payload = payload;
    }
}

public class FailureResult<T> : Result<T>
{
    public FailureResult(string message = "Error.", HttpStatusCode statusCode = HttpStatusCode.OK, T? payload = default(T))
    {
        Status = ServiceResult.Failure;
        StatusCode = statusCode;
        Message = message;
        Payload = payload;
    }
}

public enum ServiceResult
{
    Success = 0,
    Failure = 1,
}

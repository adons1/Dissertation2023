using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services;
/// <summary>
/// Определяет интерфейс методов HTTP, которые используются для отправки сервисами запросов с токенами клиентов или сервисов
/// </summary>
public interface IAuthorizedHttp
{
    public Task<Result<TResult>> GetAuthorizedAsync<TResult>(string url, object parametres);
    public Task<Result<TResult>> PostAuthorizedAsync<TResult>(string url, object? header = null, object? query = null, object? body = null);
}

using System.Net;

namespace Sidio.Mediator.Http;

/// <summary>
/// This interface is used to mark a generic HTTP result.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IHttpResult<out TResponse> : IResult<TResponse>
{
    /// <summary>
    /// Gets the HTTP status code.
    /// </summary>
    HttpStatusCode HttpStatusCode { get; }

    /// <summary>
    /// Gets a value indicating whether the HTTP status code is 200 OK.
    /// </summary>
    bool IsHttp200Ok { get; }
}
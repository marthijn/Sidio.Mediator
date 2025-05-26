namespace Sidio.Mediator.Http;

/// <summary>
/// This interface is used to mark a generic HTTP request handler.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IHttpRequestHandler<in TRequest, TResponse>
    where TRequest : IHttpRequest<TResponse>
{
    /// <summary>
    /// Handle the request asynchronously.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response.</returns>
    Task<HttpResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
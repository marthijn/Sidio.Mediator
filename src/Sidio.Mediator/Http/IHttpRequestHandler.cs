namespace Sidio.Mediator.Http;

public interface IHttpRequestHandler<TRequest, TResponse>
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
namespace Sidio.Mediator;

/// <summary>
/// The request handler interface.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">Te response type.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handle the request asynchronously.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response.</returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
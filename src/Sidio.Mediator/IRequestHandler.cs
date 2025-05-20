namespace Sidio.Mediator;

/// <summary>
/// The request handler interface.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// Handle the request asynchronously.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/>.</returns>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
namespace Sidio.Mediator;

/// <summary>
/// This interface is used to mark a request that returns a response.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IRequest<TResponse>;
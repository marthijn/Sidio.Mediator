namespace Sidio.Mediator.Http;

/// <summary>
/// This interface is used to mark a generic HTTP request handler.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IHttpRequest<TResponse>;
namespace Sidio.Mediator;

/// <summary>
/// This interface is used to mark a generic result.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface IResult<out TResponse> : IResult
{
    /// <summary>
    /// Gets the response value.
    /// </summary>
    TResponse? Value { get; }
}
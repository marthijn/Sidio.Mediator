namespace Sidio.Mediator;

/// <summary>
/// A validation error.
/// </summary>
public sealed class ValidationError
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public required string ErrorCode { get; init; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public required string ErrorMessage { get; init; }

    /// <summary>
    /// Gets the property name that caused the error.
    /// </summary>
    public required string PropertyName { get; init; }
}
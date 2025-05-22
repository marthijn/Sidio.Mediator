namespace Sidio.Mediator;

/// <summary>
/// This interface is used to mark a result.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether the result is successful.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    string? ErrorCode { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    string? ErrorMessage { get; }
}
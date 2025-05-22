namespace Sidio.Mediator;

/// <summary>
/// This class represents the result of a request.
/// </summary>
public class Result : IResult
{
    private Result(
        bool isSuccess,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError> validationErrors)
    {
        IsSuccess = isSuccess;
        ValidationErrors = validationErrors.ToList().AsReadOnly();
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    /// <inheritdoc />
    public virtual bool IsSuccess { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    /// <inheritdoc />
    public string? ErrorCode { get; }

    /// <inheritdoc />
    public string? ErrorMessage { get; }

    /// <summary>
    /// Creates a new instance of <see cref="Result"/> with the specified validation errors.
    /// </summary>
    /// <param name="validationErrors">The validation errors.</param>
    /// <returns>A <see cref="Result"/>.</returns>
    public static Result Failure(IEnumerable<ValidationError> validationErrors) =>
        new(false, null, null, validationErrors);

    /// <summary>
    /// Creates a new instance of <see cref="Result"/> with the specified error code and error message.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="validationErrors">The validation errors.</param>
    /// <returns>A <see cref="Result"/>.</returns>
    public static Result Failure(string? errorCode, string? errorMessage, IEnumerable<ValidationError>? validationErrors = null) =>
        new(false, errorCode, errorMessage, validationErrors ?? []);

    /// <summary>
    /// Creates a new instance of <see cref="Result"/> with a success status.
    /// </summary>
    /// <returns>A <see cref="Result"/>.</returns>
    public static Result Success() =>
        new(true, null, null, []);
}
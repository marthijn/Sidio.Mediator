namespace Sidio.Mediator;

/// <summary>
/// This class represents the result of a request.
/// </summary>
public class Result
{
    protected Result(
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

    /// <summary>
    /// Gets a value indicating whether the result is successful.
    /// </summary>
    public virtual bool IsSuccess { get; }

    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string? ErrorMessage { get; }

    public static Result Failure(IEnumerable<ValidationError> validationErrors) =>
        new(false, null, null, validationErrors);

    public static Result Failure(string? errorCode, string? errorMessage, IEnumerable<ValidationError>? validationErrors = null) =>
        new(false, errorCode, errorMessage, validationErrors ?? []);

    public static Result Success() =>
        new(true, null, null, []);
}
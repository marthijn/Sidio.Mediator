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

    public static Result Failure(IEnumerable<ValidationError> validationErrors) =>
        new(false, null, null, validationErrors);

    public static Result Failure(string? errorCode, string? errorMessage, IEnumerable<ValidationError>? validationErrors = null) =>
        new(false, errorCode, errorMessage, validationErrors ?? []);

    public static Result Success() =>
        new(true, null, null, []);
}
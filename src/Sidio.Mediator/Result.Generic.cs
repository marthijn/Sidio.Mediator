using System.Diagnostics.CodeAnalysis;

namespace Sidio.Mediator;

/// <summary>
/// This class represents the result of a request.
/// </summary>
public class Result<TResponse> : IResult<TResponse>
{
    private Result(
        TResponse? value,
        bool isSuccess,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError> validationErrors)
    {
        Value = value;
        IsSuccess = isSuccess;
        ValidationErrors = validationErrors.ToList().AsReadOnly();
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    /// <inheritdoc />
    public TResponse? Value { get; }

    /// <inheritdoc />
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    /// <inheritdoc />
    public string? ErrorCode { get; }

    /// <inheritdoc />
    public string? ErrorMessage { get; }

    /// <summary>
    /// Creates a new instance of <see cref="Result{TResponse}"/> with the specified validation errors.
    /// </summary>
    /// <param name="validationErrors">The validation errors.</param>
    /// <returns>A <see cref="Result{TResponse}"/>.</returns>
    public static Result<TResponse> Failure(IEnumerable<ValidationError> validationErrors) =>
        new(default!, false, null, null, validationErrors);

    /// <summary>
    /// Creates a new instance of <see cref="Result{TResponse}"/> with the specified error code and error message.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="validationErrors">The validation errors.</param>
    /// <returns>A <see cref="Result{TResponse}"/>.</returns>
    public static Result<TResponse> Failure(string? errorCode, string? errorMessage, IEnumerable<ValidationError>? validationErrors = null) =>
        new(default!, false, errorCode, errorMessage, validationErrors ?? []);

    /// <summary>
    /// Creates a new instance of <see cref="Result{TResponse}"/> with a response value and a success status.
    /// </summary>
    /// <param name="value">The response value.</param>
    /// <returns>A <see cref="Result{TResponse}"/>.</returns>
    public static Result<TResponse> Success(TResponse value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new Result<TResponse>(value, true, null, null, []);
    }
}
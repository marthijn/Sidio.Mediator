using System.Diagnostics.CodeAnalysis;

namespace Sidio.Mediator;

/// <summary>
/// This class represents the result of a request.
/// </summary>
public class Result<TResponse> : Result
{
    protected Result(
        TResponse? value,
        bool isSuccess,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError> validationErrors) : base(
        isSuccess,
        errorCode,
        errorMessage,
        validationErrors)
    {
        Value = value;
        IsSuccess = isSuccess;
    }

    /// <summary>
    /// Gets the response value.
    /// </summary>
    public virtual TResponse? Value { get; }

    /// <inheritdoc />
    [MemberNotNullWhen(true, nameof(Value))]
    public override bool IsSuccess { get; }

    public static Result<TResponse> Failure(IEnumerable<ValidationError> validationErrors) =>
        new(default!, false, null, null, validationErrors);

    public static Result<TResponse> Failure(string? errorCode, string? errorMessage, IEnumerable<ValidationError>? validationErrors = null) =>
        new(default!, false, errorCode, errorMessage, validationErrors ?? []);

    public static Result<TResponse> Success(TResponse value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new Result<TResponse>(value, true, null, null, []);
    }
}
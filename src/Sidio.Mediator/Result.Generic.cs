using System.Diagnostics.CodeAnalysis;

namespace Sidio.Mediator;

/// <summary>
/// This class represents the result of a request.
/// </summary>
public class Result<TResponse> : Result
{
    protected Result(
        TResponse? response,
        bool isSuccess,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError> validationErrors) : base(
        isSuccess,
        errorCode,
        errorMessage,
        validationErrors)
    {
        Response = response;
    }

    /// <summary>
    /// Gets the response.
    /// </summary>
    public virtual TResponse? Response { get; }

    /// <inheritdoc />
    [MemberNotNullWhen(true, nameof(Response))]
    public override bool IsSuccess { get; }

    public static Result<TResponse> Failure(IEnumerable<ValidationError> validationErrors) =>
        new(default!, false, null, null, validationErrors);

    public static Result<TResponse> Failure(string? errorCode, string? errorMessage, IEnumerable<ValidationError>? validationErrors = null) =>
        new(default!, false, errorCode, errorMessage, validationErrors ?? []);

    public static Result<TResponse> Success(TResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);
        return new Result<TResponse>(response, true, null, null, []);
    }
}
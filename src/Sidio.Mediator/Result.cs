namespace Sidio.Mediator;

public class Result
{
    internal Result(bool isSuccess, IEnumerable<ValidationError> validationErrors)
    {
        IsSuccess = isSuccess;
        ValidationErrors = validationErrors.ToList().AsReadOnly();
    }

    public bool IsSuccess { get; }

    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    public static Result Failure(IEnumerable<ValidationError> validationErrors) =>
        new(false, validationErrors);

    public static Result Success() =>
        new(true, []);
}

public sealed class Result<TResponse> : Result
{
    internal Result(TResponse response, bool isSuccess, IEnumerable<ValidationError> validationErrors) : base(
        isSuccess,
        validationErrors)
    {
        Response = response;
    }

    public TResponse Response { get; }

    public static Result<TResponse> Failure(IEnumerable<ValidationError> validationErrors) =>
        new(default!, false, validationErrors);

    public static Result<TResponse> Success(TResponse response) =>
        new(response, true, []);
}
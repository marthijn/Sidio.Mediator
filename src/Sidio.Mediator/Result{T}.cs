namespace Sidio.Mediator;

public class Result<TResponse> : Result
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
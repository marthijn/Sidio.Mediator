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
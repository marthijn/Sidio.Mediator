namespace Sidio.Mediator;

public sealed class ValidationError
{
    public required string ErrorCode { get; init; }

    public required string ErrorMessage { get; init; }

    public required string PropertyName { get; init; }
}
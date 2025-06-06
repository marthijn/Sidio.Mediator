using FluentValidation;

namespace Sidio.Mediator.SourceGenerator.IntegrationTests.Requests;

public sealed class RequestWithValidationValidator : AbstractValidator<RequestWithValidation>
{
    public RequestWithValidationValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
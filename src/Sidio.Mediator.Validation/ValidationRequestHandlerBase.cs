using FluentValidation;
using FluentValidation.Results;

namespace Sidio.Mediator.Validation;

internal abstract class ValidationRequestHandlerBase<TRequest>
{
    protected ValidationRequestHandlerBase(IEnumerable<IValidator<TRequest>> validators)
    {
        Validators = validators.ToList();
    }

    protected IReadOnlyCollection<IValidator<TRequest>> Validators { get; }

    protected async Task<ValidationFailure[]> ValidateRequestAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            Validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }
}
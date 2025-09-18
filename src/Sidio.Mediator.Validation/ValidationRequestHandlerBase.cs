using System.Collections.ObjectModel;
using FluentValidation;
using FluentValidation.Results;

namespace Sidio.Mediator.Validation;

internal abstract class ValidationRequestHandlerBase<TRequest>
{
    private readonly ReadOnlyCollection<IValidator<TRequest>> _validators;

    protected ValidationRequestHandlerBase(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators.ToList().AsReadOnly();
    }

    protected bool HasValidators => _validators.Count > 0;

    protected static IEnumerable<ValidationError> Map(IEnumerable<ValidationFailure> failures) =>
        failures.Select(f => new ValidationError
        {
            ErrorCode = f.ErrorCode,
            ErrorMessage = f.ErrorMessage,
            PropertyName = f.PropertyName,
        });

    protected async Task<IReadOnlyCollection<ValidationFailure>> ValidateRequestAsync(
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

        return validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToList();
    }
}
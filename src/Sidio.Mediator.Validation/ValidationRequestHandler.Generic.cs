using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Sidio.Mediator.Validation;

internal sealed class ValidationRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _innerHandler;
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationRequestHandler<TRequest, TResponse>> _logger;

    public ValidationRequestHandler(
        IRequestHandler<TRequest, TResponse> innerHandler,
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationRequestHandler<TRequest, TResponse>> logger)
    {
        _innerHandler = innerHandler;
        _validators = validators;
        _logger = logger;
    }

    public Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (!_validators.Any())
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(
                    "No validators found for request type {RequestType}, skipping validation",
                    typeof(TRequest).Name);
            }

            return _innerHandler.HandleAsync(request, cancellationToken);
        }

        return HandleInternalAsync(request, cancellationToken);
    }

    private async Task<Result<TResponse>> HandleInternalAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var validationFailures = await ValidateRequestAsync(request, cancellationToken);
        if (validationFailures.Length > 0)
        {
            var validationErrors = validationFailures
                .Select(x => new ValidationError
                {
                    ErrorCode = x.ErrorCode,
                    ErrorMessage = x.ErrorMessage,
                    PropertyName = x.PropertyName,
                });

            return Result<TResponse>.Failure(validationErrors);
        }

        return await _innerHandler.HandleAsync(request, cancellationToken);
    }

    private async Task<ValidationFailure[]> ValidateRequestAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }
}
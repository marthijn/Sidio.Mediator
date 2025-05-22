using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Sidio.Mediator.Validation;

internal sealed class ValidationRequestHandler<TRequest> : ValidationRequestHandlerBase<TRequest>, IRequestHandler<TRequest>
    where TRequest : IRequest
{
    private readonly IRequestHandler<TRequest> _innerHandler;
    private readonly ILogger<ValidationRequestHandler<TRequest>> _logger;

    public ValidationRequestHandler(
        IRequestHandler<TRequest> innerHandler,
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationRequestHandler<TRequest>> logger)
        : base(validators)
    {
        _innerHandler = innerHandler;
        _logger = logger;
    }

    public Task<Result> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (Validators.Count == 0)
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

    private async Task<Result> HandleInternalAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var validationFailures = await ValidateRequestAsync(request, cancellationToken);
        if (validationFailures.Length > 0)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(
                    "Validation failed for request type {RequestType}: {ValidationFailures}",
                    typeof(TRequest).Name,
                    string.Join(", ", validationFailures.Select(x => x.ErrorMessage)));
            }

            var validationErrors = validationFailures
                .Select(x => new ValidationError
                {
                    ErrorCode = x.ErrorCode,
                    ErrorMessage = x.ErrorMessage,
                    PropertyName = x.PropertyName,
                });

            return Result.Failure(validationErrors);
        }

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(
                "Validation succeeded for request type {RequestType}",
                typeof(TRequest).Name);
        }

        return await _innerHandler.HandleAsync(request, cancellationToken);
    }
}
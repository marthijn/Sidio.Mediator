using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Sidio.Mediator.Validation;

internal sealed class ValidationRequestHandler<TRequest, TResponse> : ValidationRequestHandlerBase<TRequest>, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _innerHandler;
    private readonly ILogger<ValidationRequestHandler<TRequest, TResponse>> _logger;

    public ValidationRequestHandler(
        IRequestHandler<TRequest, TResponse> innerHandler,
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationRequestHandler<TRequest, TResponse>> logger)
        : base(validators)
    {
        _innerHandler = innerHandler;
        _logger = logger;
    }

    public Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
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

    private async Task<Result<TResponse>> HandleInternalAsync(TRequest request, CancellationToken cancellationToken = default)
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

            return Result<TResponse>.Failure(validationErrors);
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
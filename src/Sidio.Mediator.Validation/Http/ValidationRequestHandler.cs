using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Sidio.Mediator.Http;

namespace Sidio.Mediator.Validation.Http;

internal sealed class ValidationHttpRequestHandler<TRequest, TResponse> : IHttpRequestHandler<TRequest, TResponse>
    where TRequest : IHttpRequest<TResponse>
{
    private readonly IHttpRequestHandler<TRequest, TResponse> _innerHandler;
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationHttpRequestHandler<TRequest, TResponse>> _logger;

    public ValidationHttpRequestHandler(
        IHttpRequestHandler<TRequest, TResponse> innerHandler,
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationHttpRequestHandler<TRequest, TResponse>> logger)
    {
        _innerHandler = innerHandler;
        _validators = validators;
        _logger = logger;
    }

    public Task<HttpResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
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

    private async Task<HttpResult<TResponse>> HandleInternalAsync(TRequest request, CancellationToken cancellationToken = default)
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

            return HttpResult<TResponse>.Failure(HttpStatusCode.BadRequest, validationErrors);
        }

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(
                "Validation succeeded for request type {RequestType}",
                typeof(TRequest).Name);
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
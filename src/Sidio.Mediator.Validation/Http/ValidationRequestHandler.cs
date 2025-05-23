using System.Net;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Sidio.Mediator.Http;

namespace Sidio.Mediator.Validation.Http;

internal sealed class ValidationHttpRequestHandler<TRequest, TResponse> : ValidationRequestHandlerBase<TRequest>, IHttpRequestHandler<TRequest, TResponse>
    where TRequest : IHttpRequest<TResponse>
{
    private readonly IHttpRequestHandler<TRequest, TResponse> _innerHandler;
    private readonly ILogger<ValidationHttpRequestHandler<TRequest, TResponse>> _logger;

    public ValidationHttpRequestHandler(
        IHttpRequestHandler<TRequest, TResponse> innerHandler,
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationHttpRequestHandler<TRequest, TResponse>> logger)
        : base(validators)
    {
        _innerHandler = innerHandler;
        _logger = logger;
    }

    public Task<HttpResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (!HasValidators)
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
        if (validationFailures.Count > 0)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(
                    "Validation failed for request type {RequestType}: {ValidationFailures}",
                    typeof(TRequest).Name,
                    string.Join(", ", validationFailures.Select(x => x.ErrorMessage)));
            }

            var validationErrors = Map(validationFailures);
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
}
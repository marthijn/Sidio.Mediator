using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Sidio.Mediator.Http;

/// <summary>
/// This class represents the result of an HTTP request.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class HttpResult<TResponse> : Result<TResponse>
{
    private HttpResult(
        TResponse? value,
        HttpStatusCode httpStatusCode,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError> validationErrors)
        : base(value, IsSuccessStatusCode(httpStatusCode), errorCode, errorMessage, validationErrors)
    {
        HttpStatusCode = httpStatusCode;
        Value = value;
    }

    /// <inheritdoc />
    public override TResponse? Value { get; }

    /// <summary>
    /// Gets the HTTP status code.
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; }

    /// <summary>
    /// Gets a value indicating whether the HTTP status code is 200 OK.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsHttp200Ok => HttpStatusCode == HttpStatusCode.OK;

    public static HttpResult<TResponse> StatusCode(
        HttpStatusCode httpStatusCode) =>
        new(default, httpStatusCode, null, null, []);

    public static HttpResult<TResponse> Failure(
        HttpStatusCode httpStatusCode,
        IEnumerable<ValidationError> validationErrors,
        TResponse? value = default) =>
        new(value, httpStatusCode, null, null, validationErrors);

    public static HttpResult<TResponse> Failure(
        HttpStatusCode httpStatusCode,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError>? validationErrors = null,
        TResponse? value = default) =>
        new(value, httpStatusCode, errorCode, errorMessage, validationErrors ?? []);

    public static HttpResult<TResponse> Unauthorized(
        string? errorCode = null,
        string? errorMessage = null,
        TResponse? value = default) =>
        new(value, HttpStatusCode.Unauthorized, errorCode, errorMessage, []);

    public static HttpResult<TResponse> BadRequest(
        IEnumerable<ValidationError> validationErrors,
        string? errorCode = null,
        string? errorMessage = null,
        TResponse? value = default) =>
        new(value, HttpStatusCode.BadRequest, errorCode, errorMessage, validationErrors);

    public static HttpResult<TResponse> NoContent(
        HttpStatusCode httpStatusCode = HttpStatusCode.NoContent) =>
        new(default, httpStatusCode, null, null, []);

    public static HttpResult<TResponse> Ok(TResponse value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HttpResult<TResponse>(value, HttpStatusCode.OK, null, null, []);
    }

    public static HttpResult<TResponse> Success(
        TResponse? value,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK) =>
        new(value, httpStatusCode, null, null, []);

    private static bool IsSuccessStatusCode(HttpStatusCode httpStatusCode) =>
        ((int) httpStatusCode >= 200) && ((int) httpStatusCode <= 299);
}
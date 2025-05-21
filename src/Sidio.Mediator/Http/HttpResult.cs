using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Sidio.Mediator.Http;

/// <summary>
/// This class represents the result of an HTTP request.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public sealed class HttpResult<TResponse> : Result<TResponse>
{
    private HttpResult(
        TResponse? response,
        HttpStatusCode httpStatusCode,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError> validationErrors)
        : base(response, IsSuccessStatusCode(httpStatusCode), errorCode, errorMessage, validationErrors)
    {
        HttpStatusCode = httpStatusCode;
        Response = response;
    }

    /// <inheritdoc />
    public override TResponse? Response { get; }

    /// <summary>
    /// Gets the HTTP status code.
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; }

    /// <summary>
    /// Gets a value indicating whether the HTTP status code is 200 OK.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Response))]
    public bool IsHttp200Ok => HttpStatusCode == HttpStatusCode.OK;

    public static HttpResult<TResponse> StatusCode(
        HttpStatusCode httpStatusCode) =>
        new(default, httpStatusCode, null, null, []);

    public static HttpResult<TResponse> Failure(
        HttpStatusCode httpStatusCode,
        IEnumerable<ValidationError> validationErrors,
        TResponse? response = default) =>
        new(response, httpStatusCode, null, null, validationErrors);

    public static HttpResult<TResponse> Failure(
        HttpStatusCode httpStatusCode,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError>? validationErrors = null,
        TResponse? response = default) =>
        new(response, httpStatusCode, errorCode, errorMessage, validationErrors ?? []);

    public static HttpResult<TResponse> Unauthorized(
        string? errorCode = null,
        string? errorMessage = null,
        TResponse? response = default) =>
        new(response, HttpStatusCode.Unauthorized, errorCode, errorMessage, []);

    public static HttpResult<TResponse> BadRequest(
        IEnumerable<ValidationError> validationErrors,
        string? errorCode = null,
        string? errorMessage = null,
        TResponse? response = default) =>
        new(response, HttpStatusCode.BadRequest, errorCode, errorMessage, validationErrors);

    public static HttpResult<TResponse> NoContent(
        HttpStatusCode httpStatusCode = HttpStatusCode.NoContent) =>
        new(default, httpStatusCode, null, null, []);

    public static HttpResult<TResponse> Ok(TResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);
        return new HttpResult<TResponse>(response, HttpStatusCode.OK, null, null, []);
    }

    public static HttpResult<TResponse> Success(
        TResponse? response,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK) =>
        new(response, httpStatusCode, null, null, []);

    private static bool IsSuccessStatusCode(HttpStatusCode httpStatusCode) =>
        ((int) httpStatusCode >= 200) && ((int) httpStatusCode <= 299);
}
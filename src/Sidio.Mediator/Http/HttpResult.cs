using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Sidio.Mediator.Http;

/// <summary>
/// This class represents the result of an HTTP request.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class HttpResult<TResponse> : IHttpResult<TResponse>
{
    private HttpResult(
        TResponse? value,
        HttpStatusCode httpStatusCode,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError> validationErrors)
    {
        HttpStatusCode = httpStatusCode;
        Value = value;
        IsSuccess = IsSuccessStatusCode(httpStatusCode);
        ValidationErrors = validationErrors.ToList().AsReadOnly();
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    /// <inheritdoc />
    public TResponse? Value { get; }

    /// <inheritdoc />
    public HttpStatusCode HttpStatusCode { get; }

    /// <inheritdoc />
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsHttp200Ok => HttpStatusCode == HttpStatusCode.OK;

    /// <inheritdoc />
    public bool IsSuccess { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    /// <inheritdoc />
    public string? ErrorCode { get; }

    /// <inheritdoc />
    public string? ErrorMessage { get; }

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with the specified HTTP status code.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> StatusCode(
        HttpStatusCode httpStatusCode) =>
        new(default, httpStatusCode, null, null, []);

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with the specified HTTP status code and validation errors.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="validationErrors">The validation errors.</param>
    /// <param name="value">The response value.</param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> Failure(
        HttpStatusCode httpStatusCode,
        IEnumerable<ValidationError> validationErrors,
        TResponse? value = default) =>
        new(value, httpStatusCode, null, null, validationErrors);

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with the specified HTTP status code, error code, and error message.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="validationErrors">The validation errors.</param>
    /// <param name="value">The response value.</param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> Failure(
        HttpStatusCode httpStatusCode,
        string? errorCode,
        string? errorMessage,
        IEnumerable<ValidationError>? validationErrors = null,
        TResponse? value = default) =>
        new(value, httpStatusCode, errorCode, errorMessage, validationErrors ?? []);

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with the Unauthorized status code.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="value">The response value.</param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> Unauthorized(
        string? errorCode = null,
        string? errorMessage = null,
        TResponse? value = default) =>
        new(value, HttpStatusCode.Unauthorized, errorCode, errorMessage, []);

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with the Bad Request status code.
    /// </summary>
    /// <param name="validationErrors">The validation errors.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorMessage"></param>
    /// <param name="value"></param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> BadRequest(
        IEnumerable<ValidationError> validationErrors,
        string? errorCode = null,
        string? errorMessage = null,
        TResponse? value = default) =>
        new(value, HttpStatusCode.BadRequest, errorCode, errorMessage, validationErrors);

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with the Bad Request status code.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorMessage"></param>
    /// <param name="value"></param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> BadRequest(
        string? errorCode = null,
        string? errorMessage = null,
        TResponse? value = default) =>
        new(value, HttpStatusCode.BadRequest, errorCode, errorMessage, []);

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with the No-Content status code.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> NoContent(
        HttpStatusCode httpStatusCode = HttpStatusCode.NoContent) =>
        new(default, httpStatusCode, null, null, []);

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with the OK status code.
    /// </summary>
    /// <param name="value">The response value.</param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> Ok(TResponse value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new HttpResult<TResponse>(value, HttpStatusCode.OK, null, null, []);
    }

    /// <summary>
    /// Creates a new instance of <see cref="HttpResult{TResponse}"/> with a response value.
    /// </summary>
    /// <param name="value">The response value.</param>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <returns>A <see cref="HttpResult{TResponse}"/>.</returns>
    public static HttpResult<TResponse> Success(
        TResponse? value,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK) =>
        new(value, httpStatusCode, null, null, []);

    private static bool IsSuccessStatusCode(HttpStatusCode httpStatusCode) =>
        ((int) httpStatusCode >= 200) && ((int) httpStatusCode <= 299);
}
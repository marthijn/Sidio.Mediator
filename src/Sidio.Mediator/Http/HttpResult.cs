using System.Net;

namespace Sidio.Mediator.Http;

public sealed class HttpResult<TResponse> : Result<TResponse>
{
    internal HttpResult(
        TResponse response,
        HttpStatusCode httpStatusCode,
        IEnumerable<ValidationError> validationErrors)
        : base(response, IsSuccessStatusCode(httpStatusCode), validationErrors)
    {
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// Gets the HTTP status code.
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; }

    public static HttpResult<TResponse> Failure(
        HttpStatusCode httpStatusCode,
        IEnumerable<ValidationError> validationErrors,
        TResponse? response = default) =>
        new(response!, httpStatusCode, validationErrors);

    public static HttpResult<TResponse> Success(
        TResponse response,
        HttpStatusCode httpStatusCode = HttpStatusCode.OK) =>
        new(response, httpStatusCode, []);

    private static bool IsSuccessStatusCode(HttpStatusCode httpStatusCode) =>
        ((int) httpStatusCode >= 200) && ((int) httpStatusCode <= 299);
}
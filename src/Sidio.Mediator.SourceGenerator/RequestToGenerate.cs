using System.Text.RegularExpressions;

namespace Sidio.Mediator.SourceGenerator;

internal readonly record struct RequestToGenerate
{
    private RequestToGenerate(
        string className,
        string requestInterface,
        string namespaceName,
        string requestHandlerInterface,
        string? returnType = null)
    {
        ClassName = className;
        RequestInterface = requestInterface;
        NamespaceName = namespaceName;
        ReturnType = returnType;
        RequestHandlerInterface = requestHandlerInterface;
    }

    public string ClassName { get; }

    public string RequestInterface { get; }

    public string NamespaceName { get; }

    public string? ReturnType { get; }

    public string RequestHandlerInterface { get; }

    public bool IsHttpRequest => RequestInterface.StartsWith("IHttpRequest");

    /// <summary>
    /// Creates a new instance of <see cref="RequestToGenerate"/> based on the provided class name, request interface, and namespace.
    /// </summary>
    /// <param name="className">The name of the class. E.g. MyRequest.</param>
    /// <param name="requestInterface">The interface of the request. E.g. IRequest{string}.</param>
    /// <param name="namespaceName">The namespace of the request.</param>
    /// <returns>A <see cref="RequestToGenerate"/>.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static RequestToGenerate Create(string className, string requestInterface, string namespaceName)
    {
        var requestMatch = Regex.Match(requestInterface, "IRequest<(?<type>[^>]+)>");
        if (requestMatch.Success)
        {
            return new RequestToGenerate(
                className,
                requestInterface,
                namespaceName,
                $"IRequestHandler<{className}, {requestMatch.Groups["type"].Value}>",
                requestMatch.Groups["type"].Value);
        }

        var httpRequestMatch = Regex.Match(requestInterface, "IHttpRequest<(?<type>[^>]+)>");
        if (httpRequestMatch.Success)
        {
            return new RequestToGenerate(
                className,
                requestInterface,
                namespaceName,
                $"IHttpRequestHandler<{className}, {httpRequestMatch.Groups["type"].Value}>",
                httpRequestMatch.Groups["type"].Value);
        }

        if (requestInterface == "IRequest")
        {
            return new RequestToGenerate(
                className,
                requestInterface,
                namespaceName,
                $"IRequestHandler<{className}>");
        }

        throw new ArgumentException($"Unsupported interface type: {requestInterface}", nameof(requestInterface));
    }
}
﻿namespace Sidio.Mediator.SourceGenerator;

internal readonly record struct RequestToGenerate
{
    private RequestToGenerate(
        string className,
        string requestInterface,
        string namespaceName,
        string requestHandlerInterface,
        ISet<string> usings,
        string? returnType = null)
    {
        ClassName = className;
        RequestInterface = requestInterface;
        NamespaceName = namespaceName;
        ReturnType = returnType;
        RequestHandlerInterface = requestHandlerInterface;
        Usings = usings;
    }

    public string ClassName { get; }

    public string RequestInterface { get; }

    public string NamespaceName { get; }

    public string? ReturnType { get; }

    public string RequestHandlerInterface { get; }

    public ISet<string> Usings { get; }

    public bool IsHttpRequest => RequestInterface.StartsWith("IHttpRequest");

    /// <summary>
    /// Creates a new instance of <see cref="RequestToGenerate"/> based on the provided class name, request interface, and namespace.
    /// </summary>
    /// <param name="className">The name of the class. E.g. MyRequest.</param>
    /// <param name="requestInterface">The interface of the request. E.g. IRequest{string}.</param>
    /// <param name="namespaceName">The namespace of the request.</param>
    /// <param name="usings">The usings.</param>
    /// <param name="genericReturnType">The generic return type.</param>
    /// <returns>A <see cref="RequestToGenerate"/>.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static RequestToGenerate Create(
        string className,
        string requestInterface,
        string namespaceName,
        ISet<string> usings,
        string? genericReturnType = null)
    {
        if (requestInterface.StartsWith("IRequest<"))
        {
            return new RequestToGenerate(
                className,
                requestInterface,
                namespaceName,
                $"IRequestHandler<{className}, {genericReturnType}>",
                usings,
                genericReturnType);
        }

        if (requestInterface.StartsWith("IHttpRequest<"))
        {
            return new RequestToGenerate(
                className,
                requestInterface,
                namespaceName,
                $"IHttpRequestHandler<{className}, {genericReturnType}>",
                usings,
                genericReturnType);
        }

        if (requestInterface == "IRequest")
        {
            return new RequestToGenerate(
                className,
                requestInterface,
                namespaceName,
                $"IRequestHandler<{className}>",
                usings);
        }

        throw new ArgumentException($"Unsupported interface type: {requestInterface}", nameof(requestInterface));
    }
}
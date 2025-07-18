﻿using Microsoft.Extensions.DependencyInjection;
using Sidio.Mediator.Http;

namespace Sidio.Mediator;

/// <summary>
/// This class contains extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the request handlers to the specified <see cref="IServiceCollection"/>.
    /// This method scans the specified assemblies for classes that implement
    /// the <see cref="IRequestHandler{TRequest, TResponse}"/>, <see cref="IRequestHandler{TRequest}"/>,
    /// and <see cref="IHttpRequestHandler{TRequest, TResponse}"/> interfaces and registers
    /// them with a scoped lifetime.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblyTypes">The assembly types to scan.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddMediatorRequestHandlers(this IServiceCollection services, params Type[] assemblyTypes)
    {
        if (assemblyTypes == null || assemblyTypes.Length == 0)
        {
            throw new ArgumentException("At least one assembly type must be provided.", nameof(assemblyTypes));
        }

        if (assemblyTypes.Any(x => x == null))
        {
            throw new ArgumentException("Assembly types cannot contain null values.", nameof(assemblyTypes));
        }

        services.Scan(s => s.FromAssembliesOf(assemblyTypes)
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IHttpRequestHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
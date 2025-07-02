using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sidio.Mediator.Http;
using Sidio.Mediator.Validation.Http;

namespace Sidio.Mediator.Validation;

/// <summary>
/// This class contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all validators in specified types, and registers the <see cref="ValidationRequestHandler{TRequest,TResponse}"/>
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assemblyTypes">The assembly types to scan.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddMediatorValidation(this IServiceCollection services, params Type[] assemblyTypes)
    {
        if (assemblyTypes == null || assemblyTypes.Length == 0)
        {
            throw new ArgumentException("At least one assembly type must be provided.", nameof(assemblyTypes));
        }

        if (assemblyTypes.Any(x => x == null))
        {
            throw new ArgumentException("Assembly types cannot contain null values.", nameof(assemblyTypes));
        }

        services.AddValidatorsFromAssemblies(assemblyTypes.Select(a => a.Assembly));
        services.TryDecorate(typeof(IRequestHandler<,>), typeof(ValidationRequestHandler<,>));
        services.TryDecorate(typeof(IRequestHandler<>), typeof(ValidationRequestHandler<>));
        services.TryDecorate(typeof(IHttpRequestHandler<,>), typeof(ValidationHttpRequestHandler<,>));
        return services;
    }
}
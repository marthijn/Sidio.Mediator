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
        ArgumentNullException.ThrowIfNull(assemblyTypes);
        services.AddValidatorsFromAssemblies(assemblyTypes.Select(a => a.Assembly));
        services.TryDecorate(typeof(IRequestHandler<,>), typeof(ValidationRequestHandler<,>));
        services.TryDecorate(typeof(IRequestHandler<>), typeof(ValidationRequestHandler<>));
        services.TryDecorate(typeof(IHttpRequestHandler<,>), typeof(ValidationHttpRequestHandler<,>));
        return services;
    }
}
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sidio.Mediator.Http;
using Sidio.Mediator.Validation.Http;

namespace Sidio.Mediator.Validation;

public static class ServiceCollectionExtensions
{
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
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
        services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationRequestHandler<,>));
        services.Decorate(typeof(IRequestHandler<>), typeof(ValidationRequestHandler<>));
        services.Decorate(typeof(IHttpRequestHandler<,>), typeof(ValidationHttpRequestHandler<,>));
        return services;
    }
}
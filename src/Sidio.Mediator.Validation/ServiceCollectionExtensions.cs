using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Sidio.Mediator.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatorValidation(this IServiceCollection services, Type assemblyType)
    {
        services.AddValidatorsFromAssembly(Assembly.GetAssembly(assemblyType));
        services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationRequestHandler<,>));
        services.Decorate(typeof(IRequestHandler<>), typeof(ValidationRequestHandler<>));
        return services;
    }
}
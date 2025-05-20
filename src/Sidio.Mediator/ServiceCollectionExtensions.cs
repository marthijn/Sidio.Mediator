using Microsoft.Extensions.DependencyInjection;

namespace Sidio.Mediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Type assemblyType)
    {
        ArgumentNullException.ThrowIfNull(assemblyType);
        services.Scan(s => s.FromAssembliesOf(assemblyType)
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
}
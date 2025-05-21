using Microsoft.Extensions.DependencyInjection;
using Sidio.Mediator.Http;

namespace Sidio.Mediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] assemblyTypes)
    {
        ArgumentNullException.ThrowIfNull(assemblyTypes);
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
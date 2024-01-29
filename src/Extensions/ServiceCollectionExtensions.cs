using Geneirodan.Generics.Extensions.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Geneirodan.Generics.Extensions;

// ReSharper disable once UnusedType.Global
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServicesFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        return services.Scan(i => i.FromAssemblies(assemblies)
            .AddClasses(c => c.WithAttribute<TransientServiceAttribute>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            .AddClasses(c => c.WithAttribute<ScopedServiceAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(c => c.WithAttribute<SingletonServiceAttribute>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );
    }
}

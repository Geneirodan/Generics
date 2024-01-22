using Geneirodan.Generics.CrudService.Attributes;
using Geneirodan.Generics.Repository.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Geneirodan.Generics.CrudService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        
        services.AddRepositories();
        return services.Scan(i => i.FromCallingAssembly()
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

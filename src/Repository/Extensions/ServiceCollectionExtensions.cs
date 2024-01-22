using Geneirodan.Generics.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Geneirodan.Generics.Repository.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.Scan(i =>
            i.FromCallingAssembly()
                .AddClasses(c => c.AssignableTo<IRepositoryService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
}

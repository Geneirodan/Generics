using Geneirodan.Generics.CrudRequests.Tests.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Geneirodan.Generics.CrudRequests.Tests;

public sealed class Application : IDisposable, IAsyncDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public Application()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISomeRepository, SomeRepository>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose() => ServiceProvider.Dispose();

    public ValueTask DisposeAsync() => ServiceProvider.DisposeAsync();
}

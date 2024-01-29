using AutoFilterer.Types;
using FluentAssertions;
using Geneirodan.Generics.CrudRequests.Tests.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Geneirodan.Generics.CrudRequests.Tests;

public sealed class Tests : IDisposable, IAsyncDisposable
{
    private readonly Application _application = new();

    [Fact]
    public async void ShouldCreate()
    {
        var command = new SomeCreateCommand("New");
        var added = await SendAsync(command);
        added.ValueOrDefault.Should().BeEquivalentTo(command);
        var response = await SendAsync(new PaginatedQuery(new PaginationFilterBase()));
        response.List.Should().HaveCount(4);
        response.List.Should().ContainEquivalentOf(command);
    }
    
    [Fact]
    public async void ShouldDelete()
    {
        var command = new SomeDeleteCommand(1);
        await SendAsync(command);
        var response = await SendAsync(new PaginatedQuery(new PaginationFilterBase()));
        response.List.Should().HaveCount(2);
        response.List.Should().NotContainEquivalentOf(command);
    }
    
    [Fact]
    public async void ShouldUpdate()
    {
        var command = new SomeUpdateCommand(1, "New");
        await SendAsync(command);
        var response = await SendAsync(new PaginatedQuery(new PaginationFilterBase()));
        response.List.Should().HaveCount(3);
        response.List.Should().ContainEquivalentOf(command);
    }

    [Theory, InlineData(3)]
    public async void ShouldGetPaginated(int count)
    {
        var response = await SendAsync(new PaginatedQuery(new PaginationFilterBase()));
        response.List.Should().HaveCount(count);
    }

    private Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) =>
        _application.ServiceProvider.GetRequiredService<ISender>().Send(request);

    public void Dispose() => _application.Dispose();
    public ValueTask DisposeAsync() => _application.DisposeAsync();
}

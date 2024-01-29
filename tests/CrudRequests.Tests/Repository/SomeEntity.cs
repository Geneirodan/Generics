using Geneirodan.Generics.Repository.Abstractions;

namespace Geneirodan.Generics.CrudRequests.Tests.Repository;

public class SomeEntity : Entity<int>
{
    public string Data { get; set; } = null!;
}

using Geneirodan.Generics.Repository.Abstractions.Interfaces;

namespace Geneirodan.Generics.Repository.Abstractions;

public class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; init; } = default!;
}
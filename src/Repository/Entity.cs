using Geneirodan.Generics.Repository.Interfaces;

namespace Geneirodan.Generics.Repository;

public class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; init; } = default!;
}
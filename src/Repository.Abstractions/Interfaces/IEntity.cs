namespace Geneirodan.Generics.Repository.Abstractions.Interfaces;

public interface IEntity<out TKey> : IEntity
{
    TKey Id { get; }
}
public interface IEntity;
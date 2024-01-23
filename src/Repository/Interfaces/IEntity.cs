namespace Geneirodan.Generics.Repository.Interfaces;

public interface IEntity<out TKey>
{
    TKey Id { get; }
}

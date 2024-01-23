namespace Geneirodan.Generics.CrudService.Interfaces;

public interface IModel<out TKey>
{
    public TKey Id { get; }
}

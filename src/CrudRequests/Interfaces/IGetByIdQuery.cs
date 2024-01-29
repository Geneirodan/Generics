namespace Geneirodan.Generics.CrudRequests.Interfaces;

public interface IGetByIdQuery<out TViewModel, out TKey> : IRequest<TViewModel>
{
    public TKey Id { get; }
}

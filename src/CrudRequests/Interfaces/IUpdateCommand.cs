using FluentResults;

namespace Geneirodan.Generics.CrudRequests.Interfaces;

public interface IUpdateCommand<out TKey, TViewModel> : IRequest<Result<TViewModel>>
{
    public TKey Id { get; }
}

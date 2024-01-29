using FluentResults;

namespace Geneirodan.Generics.CrudRequests.Interfaces;

public interface IDeleteCommand<out TKey> : IRequest<Result>
{
    public TKey Id { get; }
}

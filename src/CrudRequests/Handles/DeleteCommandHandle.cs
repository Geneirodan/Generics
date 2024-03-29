using Geneirodan.Generics.CrudRequests.Interfaces;
using Geneirodan.Generics.Repository.Abstractions.Interfaces;
using System.Net;
using Result = FluentResults.Result;

namespace Geneirodan.Generics.CrudRequests.Handles;

public abstract class DeleteCommandHandle<TDeleteCommand, TEntity, TKey>(IRepository<TEntity, TKey> repository)
    : IRequestHandler<TDeleteCommand, Result>
    where TEntity : class, IEntity<TKey>
    where TDeleteCommand : IDeleteCommand<TKey>
{

    public async Task<Result> Handle(TDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsync(request.Id);

        if (entity is null)
            return Result.Fail(HttpStatusCode.NotFound.ToString());

        repository.Remove(entity);
        await repository.ConfirmAsync(cancellationToken);
        return Result.Ok();
    }
}

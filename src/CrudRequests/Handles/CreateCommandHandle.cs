using FluentResults;
using Geneirodan.Generics.CrudRequests.Interfaces;
using Geneirodan.Generics.Repository.Abstractions.Interfaces;
using Mapster;

namespace Geneirodan.Generics.CrudRequests.Handles;

public abstract class CreateCommandHandle<TCreateCommand, TEntity, TKey, TViewModel>(IRepository<TEntity, TKey> repository)
    : IRequestHandler<TCreateCommand, Result<TViewModel>>
    where TEntity : class, IEntity<TKey>
    where TCreateCommand : ICreateCommand<TViewModel>
{

    public async Task<Result<TViewModel>> Handle(TCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<TEntity>();
        var created = repository.Add(entity);
        await repository.ConfirmAsync(cancellationToken);
        var viewModel = created.Adapt<TViewModel>();
        return Result.Ok(viewModel);
    }
}

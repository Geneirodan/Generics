using Geneirodan.Generics.CrudRequests.Interfaces;
using Geneirodan.Generics.Repository.Abstractions.Interfaces;
using Geneirodan.Generics.Results;
using Mapster;
using Result = FluentResults.Result;

namespace Geneirodan.Generics.CrudRequests.Handles;

public abstract class UpdateCommandHandle<TUpdateCommand, TEntity, TKey, TViewModel>(IRepository<TEntity, TKey> repository)
    : IRequestHandler<TUpdateCommand, FluentResults.Result<TViewModel>>
    where TEntity : class, IEntity<TKey>
    where TUpdateCommand : IUpdateCommand<TKey, TViewModel>
{
    public async Task<FluentResults.Result<TViewModel>> Handle(TUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetAsync(request.Id);

        if (entity is null)
            return new NotFoundResult();

        entity = request.Adapt<TEntity>();
        repository.Update(entity);
        await repository.ConfirmAsync(cancellationToken);
        var viewModel = entity.Adapt<TViewModel>();
        return Result.Ok(viewModel);
    }
}

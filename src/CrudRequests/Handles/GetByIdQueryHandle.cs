using Geneirodan.Generics.CrudRequests.Interfaces;
using Geneirodan.Generics.Repository.Abstractions.Interfaces;
using Mapster;

namespace Geneirodan.Generics.CrudRequests.Handles;

public abstract class GetByIdQueryHandle<TQuery, TEntity, TKey, TViewModel>(IRepository<TEntity, TKey> repository)
    : IRequestHandler<TQuery, TViewModel>
    where TEntity : class, IEntity<TKey>
    where TQuery : IGetByIdQuery<TViewModel, TKey>
{
    public async Task<TViewModel> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAsync(request.Id);
        return entities.Adapt<TViewModel>();
    }
}

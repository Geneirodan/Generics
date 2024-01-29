using Geneirodan.Generics.CrudRequests.Interfaces;
using Geneirodan.Generics.Repository.Abstractions;
using Geneirodan.Generics.Repository.Abstractions.Interfaces;
using Mapster;

namespace Geneirodan.Generics.CrudRequests.Handles;

public abstract class PaginatedQueryHandle<TQuery, TEntity, TKey, TViewModel>(IRepository<TEntity, TKey> repository)
    : IRequestHandler<TQuery, PaginatedList<TViewModel>>
    where TEntity : class, IEntity<TKey>
    where TQuery : IPaginatedQuery<TViewModel>
{
    public async Task<PaginatedList<TViewModel>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAll(request.Filter);
        return entities.Adapt<PaginatedList<TViewModel>>();
    }
}

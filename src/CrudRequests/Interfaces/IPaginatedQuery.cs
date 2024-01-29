using AutoFilterer.Abstractions;
using Geneirodan.Generics.Repository.Abstractions;

namespace Geneirodan.Generics.CrudRequests.Interfaces;

public interface IPaginatedQuery<TViewModel> : IRequest<PaginatedList<TViewModel>>
{
    public IPaginationFilter Filter { get; }
}

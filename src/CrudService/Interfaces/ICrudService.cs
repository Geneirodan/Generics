using AutoFilterer.Abstractions;
using FluentResults;
using FluentValidation;
using Geneirodan.Generics.Repository.Abstractions;
using Geneirodan.Generics.Repository.Abstractions.Interfaces;

namespace Geneirodan.Generics.CrudService.Interfaces;

public interface ICrudService<in TEntity, TViewModel, in TKey> where TEntity : class, IEntity<TKey>
    where TViewModel : IViewModel<TEntity>
{
    Task<Result<TViewModel>> AddAsync<TAddModel>(TAddModel model, IValidator<TAddModel>? validator = null)
        where TAddModel : IAddModel<TEntity>;
    Task<Result> DeleteAsync(TKey id);
    Task<Result<TViewModel>> EditAsync<TEditModel>(TKey id, TEditModel model, IValidator<TEditModel>? validator = null)
        where TEditModel : IEditModel<TEntity>;
    Task<TViewModel?> GetByIdAsync(TKey id);
    Task<PaginatedList<TViewModel>> GetAsync(IPaginationFilter filter);
}

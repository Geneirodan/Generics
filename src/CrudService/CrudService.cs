using AutoFilterer.Abstractions;
using FluentResults;
using FluentValidation;
using Geneirodan.Generics.CrudService.Interfaces;
using Geneirodan.Generics.Extensions;
using Geneirodan.Generics.Repository.Abstractions;
using Geneirodan.Generics.Repository.Abstractions.Interfaces;
using Geneirodan.Generics.Results;
using Mapster;

namespace Geneirodan.Generics.CrudService;

public class CrudService<TEntity, TViewModel, TKey>(IRepository<TEntity, TKey> repository) : ICrudService<TEntity, TViewModel, TKey>
    where TEntity : class, IEntity<TKey>
    where TViewModel : IViewModel<TEntity>
{
    public virtual async Task<Result<TViewModel>> AddAsync<TAddModel>(TAddModel model, IValidator<TAddModel>? validator = null)
        where TAddModel : IAddModel<TEntity>
    {
        try
        {
            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(model);
                if (!validationResult.IsValid)
                    return validationResult.ToFluentResult();
            }
            var entity = model.Adapt<TEntity>();
            var created = repository.Add(entity);
            await repository.ConfirmAsync();
            var viewModel = created.Adapt<TViewModel>();
            return Result.Ok(viewModel);
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public virtual async Task<Result> DeleteAsync(TKey id)
    {
        try
        {
            var entity = await repository.GetAsync(id);
            if (entity is null)
                return new NotFoundResult();
            repository.Remove(entity);
            await repository.ConfirmAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public virtual async Task<Result<TViewModel>> EditAsync<TEditModel>(TKey id, TEditModel model, IValidator<TEditModel>? validator = null)
        where TEditModel : IEditModel<TEntity>
    {
        try
        {
            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(model);
                var result = validationResult.ToFluentResult();
                if (result.IsFailed)
                    return result;
            }
            var entity = await repository.GetAsync(id);
            if (entity is null)
                return new NotFoundResult();
            var config = new TypeAdapterConfig();
            config.NewConfig<TEditModel, TEntity>().IgnoreNullValues(true);
            entity.Adapt(model, config);
            repository.Update(entity);
            await repository.ConfirmAsync();
            var viewModel = entity.Adapt<TViewModel>();
            return Result.Ok(viewModel);
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public async Task<TViewModel?> GetByIdAsync(TKey id)
    {
        var entity = await repository.GetAsync(id);
        return entity.Adapt<TViewModel>();
    }

    public async Task<PaginatedList<TViewModel>> GetAsync(IPaginationFilter filter)
    {
        var entities = await repository.GetAll(filter);
        return entities.Adapt<PaginatedList<TViewModel>>();
    }
}

using AutoFilterer.Abstractions;
using AutoFilterer.Extensions;
using Geneirodan.Generics.Repository.Abstractions;
using Mapster;
using System.Linq.Expressions;

namespace Geneirodan.Generics.CrudRequests.Tests.Repository;

public class SomeRepository : ISomeRepository
{
    private readonly List<SomeEntity> _entities = [
        new SomeEntity {Id = 1, Data = "1"},
        new SomeEntity {Id = 2, Data = "2"},
        new SomeEntity {Id = 3, Data = "3"}
    ];

    private IQueryable<SomeEntity> Queryable => _entities.AsQueryable();
    
    public Task<IQueryable<SomeEntity>> GetAll() => Task.FromResult(Queryable);
    
    public SomeEntity Add(SomeEntity entity)
    {
        _entities.Add(entity);
        return Queryable.Last();
    }
    
    public void AddRange(params SomeEntity[] entities) => _entities.AddRange(entities);
    
    public Task<SomeEntity?> GetAsync(Expression<Func<SomeEntity, bool>> expression, CancellationToken cancellationToken = default) =>
        Task.FromResult(Queryable.FirstOrDefault(expression));
    
    public Task<SomeEntity?> GetAsync(int id) =>
        Task.FromResult(Queryable.FirstOrDefault(x => x.Id == id));
    
    public Task<SomeEntity?> GetAsync(object?[]? keys) =>
        Task.FromResult(Queryable.FirstOrDefault(x => keys != null && keys.Contains(x.Id)));
    public void Remove(SomeEntity entity) => _entities.Remove(entity);

    public void RemoveRange(IEnumerable<SomeEntity> entities) => throw new NotImplementedException();
    public void Update(SomeEntity entity)
    {
        var someEntity = _entities.Find(x => x.Id == entity.Id);
        entity.Adapt(someEntity);
    }
    public void UpdateRange(IEnumerable<SomeEntity> entities)
    {
        foreach (var someEntity in entities)
            Update(someEntity);
    }
    public Task<int> ConfirmAsync(CancellationToken cancellationToken = default) => Task.FromResult(0);
    public Task<IQueryable<SomeEntity>> GetAll(IFilter filter) => Task.FromResult(Queryable.ApplyFilter(filter));
    public Task<PaginatedList<SomeEntity>> GetAll(IPaginationFilter filter)
    {
        var entities = Queryable.ApplyFilterWithoutPagination(filter);
        var paged = entities.ToPaged(filter.Page, filter.PerPage);

        var paginationModel = new PaginatedList<SomeEntity>(paged, entities.Count());
        return Task.FromResult(paginationModel);
    }
}
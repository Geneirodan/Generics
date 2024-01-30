using AutoFilterer.Abstractions;
using AutoFilterer.Extensions;
using Geneirodan.Generics.Repository.Abstractions;
using Geneirodan.Generics.Repository.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Geneirodan.Generics.Repository.EntityFrameworkCore;

public abstract class Repository<TEntity>(DbContext context) : Repository<TEntity, int>(context)
    where TEntity : class, IEntity<int>;

public abstract class Repository<TEntity, TKey>(DbContext context) : Repository<TEntity, TKey, DbContext>(context)
    where TEntity : class, IEntity<TKey>;

public abstract class Repository<TEntity, TKey, TContext>(TContext context) : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TContext : DbContext
{
    // ReSharper disable once MemberCanBePrivate.Global
    protected TContext Context => context;

    private DbSet<TEntity> Entities => Context.Set<TEntity>();
    private IQueryable<TEntity> NotTrackingEntities => Entities.AsNoTracking();

    public Task<IQueryable<TEntity>> GetAll() => Task.FromResult(NotTrackingEntities);

    public Task<IQueryable<TEntity>> GetAll(IFilter filter) => Task.FromResult(NotTrackingEntities.ApplyFilter(filter));
    public Task<PaginatedList<TEntity>> GetAll(IPaginationFilter filter)
    {
        var entities = NotTrackingEntities.ApplyFilterWithoutPagination(filter);
        var paged = entities.ToPaged(filter.Page, filter.PerPage);

        var paginationModel = new PaginatedList<TEntity>(paged, entities.Count());
        return Task.FromResult(paginationModel);
    }

    public TEntity Add(TEntity entity) => Entities.Add(entity).Entity;

    public void AddRange(params TEntity[] entities) => Entities.AddRange(entities);

    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default) =>
        NotTrackingEntities.FirstOrDefaultAsync(expression, cancellationToken);

    public Task<TEntity?> GetAsync(TKey id) => GetAsync(x => Equals(x.Id, id));

    public void Remove(TEntity entity) => Entities.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => Entities.RemoveRange(entities);

    public void Update(TEntity entity) => Entities.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) => Entities.UpdateRange(entities);

    public Task<int> ConfirmAsync(CancellationToken cancellationToken = default) => Context.SaveChangesAsync(cancellationToken);
}

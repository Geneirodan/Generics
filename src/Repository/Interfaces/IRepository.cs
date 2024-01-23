using System.Linq.Expressions;

namespace Geneirodan.Generics.Repository.Interfaces;

public interface IRepository<TEntity, in TKey> : IRepositoryService
    where TEntity : class, IEntity<TKey>
{
    IQueryable<TEntity> GetAll();
    TEntity Add(TEntity entity);
    void AddRange(params TEntity[] entities);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(TKey id);
    Task<TEntity?> GetAsync(object?[]? keys);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    Task<int> ConfirmAsync(CancellationToken cancellationToken = default);
}
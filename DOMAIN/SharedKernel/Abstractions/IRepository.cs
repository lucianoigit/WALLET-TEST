namespace DOMAIN.SharedKernel.Abstractions;

public interface IRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    Task<TEntity?> GetByIdAsync(TEntityId id);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}

using DOMAIN.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace INFRAESTRUCTURE.Repositories;

internal abstract class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : IEquatable<TEntityId>
{
    protected readonly ApplicationDbContext _context;


    protected Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual Task<TEntity?> GetByIdAsync(TEntityId id)
    {
        return _context.Set<TEntity>()
            .SingleOrDefaultAsync(p => p.Id.Equals(id));
    }

    public virtual void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}

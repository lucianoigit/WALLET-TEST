namespace DOMAIN.SharedKernel.Abstractions;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : class
{
    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }

    // For EF Core purposes.
    protected Entity() { }

    // Id equality

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    public bool Equals(Entity<TId> other)
    {
        return Equals((object?)other);
    }
    // Reference equality
    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
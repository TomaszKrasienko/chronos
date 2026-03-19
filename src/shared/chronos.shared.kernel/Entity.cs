using chronos.shared.kernel.Exceptions;

namespace chronos.shared.kernel;

/// <summary>
/// Base class for domain entities with identity.
/// </summary>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
public abstract class Entity<TId> where TId : struct, IEntityId
{
    /// <summary>
    /// Gets the entity identifier.
    /// </summary>
    public TId Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Validates a business rule and throws <see cref="DomainException"/> if the rule is broken.
    /// </summary>
    /// <param name="rule">The business rule to validate.</param>
    /// <exception cref="DomainException">Thrown when the rule is broken.</exception>
    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new DomainException(rule.Code);
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);
}

using chronos.shared.kernel.Exceptions;

namespace chronos.shared.kernel;

/// <summary>
/// Base class for value objects. Value objects are immutable and compared by their properties.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Returns the components used for equality comparison.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

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
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((ValueObject)obj);
    }

    public bool Equals(ValueObject? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
                HashCode.Combine(current, obj?.GetHashCode() ?? 0));
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
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

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}

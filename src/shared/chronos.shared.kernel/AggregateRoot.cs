namespace chronos.shared.kernel;

/// <summary>
/// Base class for aggregate roots. Aggregates are consistency boundaries for domain operations.
/// </summary>
/// <typeparam name="TId">The type of the aggregate identifier.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId> where TId : struct, IEntityId
{
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }
}

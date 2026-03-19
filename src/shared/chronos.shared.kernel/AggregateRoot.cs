namespace chronos.shared.kernel;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : struct, IEntityId
{
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TId id) : base(id)
    {
    }
}

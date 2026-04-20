namespace chronos.shared.kernel.Identifiers;

/// <summary>
/// Strongly-typed identifier for the Contact aggregate.
/// </summary>
public readonly record struct ContactId(Ulid Value) : IEntityId
{
    public static ContactId New() => new(Ulid.NewUlid());
}

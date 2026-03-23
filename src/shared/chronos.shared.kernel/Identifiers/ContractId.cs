namespace chronos.shared.kernel.Identifiers;

/// <summary>
/// Strongly-typed identifier for the Contract aggregate.
/// </summary>
public readonly record struct ContractId(Ulid Value) : IEntityId
{
    public static ContractId New() => new(Ulid.NewUlid());
}

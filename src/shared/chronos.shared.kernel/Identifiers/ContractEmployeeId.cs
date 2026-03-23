namespace chronos.shared.kernel.Identifiers;

/// <summary>
/// Strongly-typed identifier for the ContractEmployee entity.
/// </summary>
public readonly record struct ContractEmployeeId(Ulid Value) : IEntityId
{
    public static ContractEmployeeId New() => new(Ulid.NewUlid());
}

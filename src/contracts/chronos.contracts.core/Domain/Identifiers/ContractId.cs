using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for <see cref="Contract"/> aggregate.
/// </summary>
public readonly record struct ContractId(Ulid Value) : IEntityId
{
    /// <summary>
    /// Creates a new unique contract identifier.
    /// </summary>
    public static ContractId New() => new(Ulid.NewUlid());
}

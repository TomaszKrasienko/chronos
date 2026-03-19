using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for <see cref="ContractEmployee"/> entity.
/// </summary>
public readonly record struct ContractEmployeeId(Ulid Value) : IEntityId
{
    /// <summary>
    /// Creates a new unique contract employee identifier.
    /// </summary>
    public static ContractEmployeeId New() => new(Ulid.NewUlid());
}

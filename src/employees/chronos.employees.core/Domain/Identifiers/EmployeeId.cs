using chronos.shared.kernel;

namespace chronos.employees.core.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for <see cref="Employee"/> aggregate.
/// </summary>
public readonly record struct EmployeeId(Ulid Value) : IEntityId
{
    /// <summary>
    /// Creates a new unique employee identifier.
    /// </summary>
    public static EmployeeId New() => new(Ulid.NewUlid());
}

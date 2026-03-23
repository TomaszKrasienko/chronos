namespace chronos.shared.kernel.Identifiers;

/// <summary>
/// Strongly-typed identifier for the Employee aggregate.
/// </summary>
public readonly record struct EmployeeId(Ulid Value) : IEntityId
{
    public static EmployeeId New() => new(Ulid.NewUlid());
}

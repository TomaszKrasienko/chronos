namespace chronos.shared.kernel.Identifiers;

/// <summary>
/// Strongly-typed identifier for the TimeLog entity.
/// </summary>
public readonly record struct TimeLogId(Ulid Value) : IEntityId
{
    public static TimeLogId New() => new(Ulid.NewUlid());
}

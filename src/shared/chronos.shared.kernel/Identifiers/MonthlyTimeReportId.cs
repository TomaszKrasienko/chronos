namespace chronos.shared.kernel.Identifiers;

/// <summary>
/// Strongly-typed identifier for the MonthlyTimeReport aggregate.
/// </summary>
public readonly record struct MonthlyTimeReportId(Ulid Value) : IEntityId
{
    public static MonthlyTimeReportId New() => new(Ulid.NewUlid());
}

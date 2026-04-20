using chronos.time_logs.core.Domain.ValueObjects;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="ReportPeriod"/> instances in tests.
/// </summary>
public static class ReportPeriodFactory
{
    public static ReportPeriod Create(
        int month = 4,
        int year = 2026)
        => ReportPeriod.Create(month, year);
}

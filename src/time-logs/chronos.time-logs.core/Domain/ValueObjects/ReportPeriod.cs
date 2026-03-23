using chronos.shared.kernel;
using chronos.time_logs.core.Domain.Rules;

namespace chronos.time_logs.core.Domain.ValueObjects;

/// <summary>
/// Value object representing a report period (month and year).
/// </summary>
public sealed class ReportPeriod : ValueObject
{
    /// <summary>
    /// Gets the month (1-12).
    /// </summary>
    public int Month { get; }

    /// <summary>
    /// Gets the year.
    /// </summary>
    public int Year { get; }

    private ReportPeriod(int month, int year)
    {
        Month = month;
        Year = year;
    }

    /// <summary>
    /// Creates a new ReportPeriod instance.
    /// </summary>
    /// <param name="month">The month (1-12).</param>
    /// <param name="year">The year.</param>
    public static ReportPeriod Create(int month, int year)
    {
        CheckRule(new ValidMonthRule(month));
        return new ReportPeriod(month, year);
    }

    /// <summary>
    /// Creates a ReportPeriod for the current month.
    /// </summary>
    /// <param name="timeProvider">The time provider for getting current time.</param>
    public static ReportPeriod Current(TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        return new ReportPeriod(now.Month, now.Year);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Month;
        yield return Year;
    }
}

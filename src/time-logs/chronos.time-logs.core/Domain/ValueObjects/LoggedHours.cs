using chronos.shared.kernel;
using chronos.time_logs.core.Domain.Rules;

namespace chronos.time_logs.core.Domain.ValueObjects;

/// <summary>
/// Value object representing logged hours.
/// </summary>
public sealed class LoggedHours : ValueObject
{
    /// <summary>
    /// Gets the time span value.
    /// </summary>
    public TimeSpan Value { get; }

    private LoggedHours(TimeSpan value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new LoggedHours instance.
    /// </summary>
    /// <param name="value">The time span value.</param>
    public static LoggedHours Create(TimeSpan value)
    {
        CheckRule(new HoursMustBePositiveRule(value));
        return new LoggedHours(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

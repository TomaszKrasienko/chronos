using chronos.shared.kernel;
using chronos.time_logs.core.Domain.Rules;

namespace chronos.time_logs.core.Domain.ValueObjects;

/// <summary>
/// Value object representing logged time.
/// </summary>
public sealed class LoggedTime : ValueObject
{
    /// <summary>
    /// Gets the time span value.
    /// </summary>
    public TimeSpan Value { get; }

#pragma warning disable CS8618
    private LoggedTime()
    {
    }
#pragma warning restore CS8618

    private LoggedTime(TimeSpan value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new LoggedTime instance.
    /// </summary>
    /// <param name="value">The time span value.</param>
    public static LoggedTime Create(TimeSpan value)
    {
        CheckRule(new TimeMustBePositiveRule(value));
        return new LoggedTime(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

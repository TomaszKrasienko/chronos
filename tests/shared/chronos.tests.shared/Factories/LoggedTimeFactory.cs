using chronos.time_logs.core.Domain.ValueObjects;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="LoggedTime"/> instances in tests.
/// </summary>
public static class LoggedTimeFactory
{
    public static LoggedTime Create(TimeSpan? value = null)
        => LoggedTime.Create(value ?? TimeSpan.FromHours(8));
}

using chronos.shared.kernel;

namespace chronos.time_logs.core.Domain.Rules;

/// <summary>
/// Rule that validates logged time must be positive.
/// </summary>
internal sealed class TimeMustBePositiveRule(TimeSpan time) : IBusinessRule
{
    public string Code => "time_must_be_positive";

    public bool IsBroken() => time <= TimeSpan.Zero;
}

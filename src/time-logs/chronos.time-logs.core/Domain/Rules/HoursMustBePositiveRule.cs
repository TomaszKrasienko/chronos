using chronos.shared.kernel;

namespace chronos.time_logs.core.Domain.Rules;

/// <summary>
/// Rule that validates logged hours must be positive.
/// </summary>
internal sealed class HoursMustBePositiveRule(TimeSpan hours) : IBusinessRule
{
    public string Code => "hours_must_be_positive";

    public bool IsBroken() => hours <= TimeSpan.Zero;
}

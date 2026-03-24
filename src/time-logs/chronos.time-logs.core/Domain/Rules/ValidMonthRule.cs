using chronos.shared.kernel;

namespace chronos.time_logs.core.Domain.Rules;

/// <summary>
/// Rule that validates month is between 1 and 12.
/// </summary>
internal sealed class ValidMonthRule(int month) : IBusinessRule
{
    public string Code => "invalid_month";

    public bool IsBroken() => month < 1 || month > 12;
}

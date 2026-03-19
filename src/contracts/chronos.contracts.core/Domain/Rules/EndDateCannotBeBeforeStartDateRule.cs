using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Rules;

/// <summary>
/// Rule that validates end date cannot be before start date.
/// </summary>
internal sealed class EndDateCannotBeBeforeStartDateRule(DateOnly from, DateOnly to) : IBusinessRule
{
    public string Code => "end_date_cannot_be_before_start_date";
    public bool IsBroken() => to < from;
}

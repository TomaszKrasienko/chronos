using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Rules;

/// <summary>
/// Rule that validates allocated hours must be greater than zero.
/// </summary>
internal sealed class AllocatedHoursMustBeGreaterThanZeroRule(int allocatedHours) : IBusinessRule
{
    public string Code => "allocated_hours_must_be_greater_than_zero";
    public bool IsBroken() => allocatedHours <= 0;
}

using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Rules;

/// <summary>
/// Rule that validates an employee assignment cannot start before the contract assignment date.
/// </summary>
internal sealed class EmployeeAssignmentCannotStartBeforeContractRule(
    DateOnly contractAssignmentDate,
    DateOnly employeeAssignmentFrom) : IBusinessRule
{
    public string Code => "employee_assignment_cannot_start_before_contract";

    public bool IsBroken() => employeeAssignmentFrom < contractAssignmentDate;
}

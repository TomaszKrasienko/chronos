using chronos.contracts.core.Domain.Identifiers;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Rules;

/// <summary>
/// Rule that validates an employee assignment must exist in the contract.
/// </summary>
internal sealed class EmployeeAssignmentMustExistRule(
    IEnumerable<ContractEmployee> employees,
    ContractEmployeeId contractEmployeeId) : IBusinessRule
{
    public string Code => "employee_assignment_not_found";
    public bool IsBroken() => !employees.Any(e => e.Id == contractEmployeeId);
}

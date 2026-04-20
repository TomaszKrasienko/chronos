using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Domain.Rules;

/// <summary>
/// Rule that validates an employee assignment must exist in the contract.
/// </summary>
internal sealed class EmployeeAssignmentMustExistRule(
    IEnumerable<ContractEmployee> employees,
    EmployeeId employeeId) : IBusinessRule
{
    public string Code => "employee_assignment_not_found";
    public bool IsBroken() => employees.All(e => e.Id != employeeId);
}

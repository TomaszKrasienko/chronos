using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Rules;

/// <summary>
/// Rule that validates an employee is not already assigned to the contract.
/// </summary>
internal sealed class EmployeeAlreadyAssignedToContractRule(
    IEnumerable<ContractEmployee> employees,
    Ulid employeeId) : IBusinessRule
{
    public string Code => "employee_already_assigned_to_contract";

    public bool IsBroken() => employees.Any(e => e.EmployeeId == employeeId);
}

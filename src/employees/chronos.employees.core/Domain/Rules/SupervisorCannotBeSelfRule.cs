using chronos.employees.core.Domain.Identifiers;
using chronos.shared.kernel;

namespace chronos.employees.core.Domain.Rules;

/// <summary>
/// Rule that validates an employee cannot be their own supervisor.
/// </summary>
internal sealed class SupervisorCannotBeSelfRule(EmployeeId employeeId, EmployeeId supervisorId) : IBusinessRule
{
    public string Code => "supervisor_cannot_be_self";
    public bool IsBroken() => employeeId == supervisorId;
}

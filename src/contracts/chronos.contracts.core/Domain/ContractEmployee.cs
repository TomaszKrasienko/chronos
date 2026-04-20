using chronos.contracts.core.Domain.Rules;
using chronos.contracts.core.Domain.ValueObjects;
using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.contracts.core.Domain;

/// <summary>
/// Entity representing an employee assigned to a contract with allocation details.
/// </summary>
public sealed class ContractEmployee : Entity<EmployeeId>
{
    /// <summary>
    /// Gets the assignment period for this employee.
    /// </summary>
    public AssignmentPeriod AssignmentPeriod { get; private set; }

    /// <summary>
    /// Gets the number of hours allocated to this employee.
    /// </summary>
    public int AllocatedHours { get; private set; }

#pragma warning disable CS8618
    private ContractEmployee()
    {
    }
#pragma warning restore CS8618

    private ContractEmployee(
        EmployeeId employeeId,
        AssignmentPeriod assignmentPeriod,
        int allocatedHours) : base(employeeId)
    {
        AssignmentPeriod = assignmentPeriod;
        AllocatedHours = allocatedHours;
    }

    internal static ContractEmployee Create(
        EmployeeId employeeId,
        AssignmentPeriod assignmentPeriod,
        int allocatedHours)
    {
        CheckRule(new AllocatedHoursMustBeGreaterThanZeroRule(allocatedHours));
        
        return new ContractEmployee(
            employeeId,
            assignmentPeriod,
            allocatedHours);
    }

    internal void UpdateAllocatedHours(int allocatedHours)
    {
        CheckRule(new AllocatedHoursMustBeGreaterThanZeroRule(allocatedHours));
        AllocatedHours = allocatedHours;
    }

    internal void UpdateAssignmentPeriod(AssignmentPeriod assignmentPeriod)
        => AssignmentPeriod = assignmentPeriod;
    
}

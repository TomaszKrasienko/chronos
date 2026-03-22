using chronos.contracts.core.Domain.Identifiers;
using chronos.contracts.core.Domain.Rules;
using chronos.contracts.core.Domain.ValueObjects;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain;

/// <summary>
/// Entity representing an employee assigned to a contract with allocation details.
/// </summary>
public sealed class ContractEmployee : Entity<ContractEmployeeId>
{
    /// <summary>
    /// Gets the employee identifier from Employee bounded context.
    /// </summary>
    public Ulid EmployeeId { get; private set; }

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
        ContractEmployeeId id,
        Ulid employeeId,
        AssignmentPeriod assignmentPeriod,
        int allocatedHours) : base(id)
    {
        EmployeeId = employeeId;
        AssignmentPeriod = assignmentPeriod;
        AllocatedHours = allocatedHours;
    }

    internal static ContractEmployee Create(
        Ulid employeeId,
        AssignmentPeriod assignmentPeriod,
        int allocatedHours)
    {
        CheckRule(new AllocatedHoursMustBeGreaterThanZeroRule(allocatedHours));
        
        return new ContractEmployee(
            ContractEmployeeId.New(),
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

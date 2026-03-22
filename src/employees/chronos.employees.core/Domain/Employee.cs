using chronos.employees.core.Domain.Identifiers;
using chronos.employees.core.Domain.Rules;
using chronos.employees.core.Domain.ValueObjects;
using chronos.shared.kernel;

namespace chronos.employees.core.Domain;

/// <summary>
/// Aggregate root representing an employee in the system.
/// </summary>
public sealed class Employee : AggregateRoot<EmployeeId>
{
    /// <summary>
    /// Gets the employee's full name.
    /// </summary>
    public FullName FullName { get; private set; }

    /// <summary>
    /// Gets the employee's email address.
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Gets the supervisor's employee identifier, if assigned.
    /// </summary>
    public EmployeeId? SupervisorId { get; private set; }

#pragma warning disable CS8618
    private Employee()
    {
    }
#pragma warning restore CS8618

    private Employee(
        EmployeeId id,
        FullName fullName,
        Email email) : base(id)
    {
        FullName = fullName;
        Email = email;
    }

    /// <summary>
    /// Creates a new employee with the specified name and email.
    /// </summary>
    /// <param name="fullName">The employee's full name.</param>
    /// <param name="email">The employee's email address.</param>
    public static Employee Create(FullName fullName, Email email)
        => new(EmployeeId.New(), fullName, email);

    /// <summary>
    /// Assigns a supervisor to this employee.
    /// </summary>
    /// <param name="supervisorId">The supervisor's employee identifier.</param>
    public void AssignSupervisor(EmployeeId supervisorId)
    {
        CheckRule(new SupervisorCannotBeSelfRule(Id, supervisorId));
        SupervisorId = supervisorId;
    }

    /// <summary>
    /// Removes the supervisor assignment from this employee.
    /// </summary>
    public void RemoveSupervisor()
    {
        SupervisorId = null;
    }
}

using chronos.employees.core.Domain;
using chronos.employees.core.Domain.ValueObjects;
using chronos.shared.kernel.Identifiers;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="Employee"/> instances in tests.
/// </summary>
public static class EmployeeFactory
{
    public static Employee Create(
        FullName? fullName = null,
        Email? email = null)
        => Employee.Create(
            fullName ?? FullNameFactory.Create(),
            email ?? EmailFactory.Create());

    public static Employee CreateWithSupervisor(
        EmployeeId? supervisorId = null,
        FullName? fullName = null,
        Email? email = null)
    {
        var employee = Create(fullName, email);
        employee.AssignSupervisor(supervisorId ?? EmployeeId.New());
        return employee;
    }
}

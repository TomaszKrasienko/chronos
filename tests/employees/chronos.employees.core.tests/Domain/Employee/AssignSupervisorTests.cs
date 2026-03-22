using chronos.employees.core.Domain.Identifiers;
using chronos.shared.kernel.Exceptions;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.employees.core.tests.Domain.Employee;

public sealed class AssignSupervisorTests
{
    [Fact]
    public void GivenValidSupervisor_WhenAssigningSupervisor_ThenSupervisorIsAssigned()
    {
        // Arrange
        var employee = EmployeeFactory.Create();
        var supervisorId = EmployeeId.New();

        // Act
        employee.AssignSupervisor(supervisorId);

        // Assert
        employee.SupervisorId.ShouldBe(supervisorId);
    }

    [Fact]
    public void GivenSelfAsSupervisor_WhenAssigningSupervisor_ThenThrowsDomainExceptionWithCode_supervisor_cannot_be_self()
    {
        // Arrange
        var employee = EmployeeFactory.Create();

        // Act
        var exception = Should.Throw<DomainException>(
            () => employee.AssignSupervisor(employee.Id));

        // Assert
        exception.Code.ShouldBe("supervisor_cannot_be_self");
    }
}

using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.employees.core.tests.Domain.Employee;

public sealed class RemoveSupervisorTests
{
    [Fact]
    public void GivenEmployeeWithSupervisor_WhenRemovingSupervisor_ThenSupervisorIdIsNull()
    {
        // Arrange
        var employee = EmployeeFactory.CreateWithSupervisor();

        // Act
        employee.RemoveSupervisor();

        // Assert
        employee.SupervisorId.ShouldBeNull();
    }

    [Fact]
    public void GivenEmployeeWithoutSupervisor_WhenRemovingSupervisor_ThenSupervisorIdRemainsNull()
    {
        // Arrange
        var employee = EmployeeFactory.Create();

        // Act
        employee.RemoveSupervisor();

        // Assert
        employee.SupervisorId.ShouldBeNull();
    }
}

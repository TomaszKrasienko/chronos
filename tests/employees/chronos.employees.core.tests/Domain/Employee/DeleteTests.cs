using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.employees.core.tests.Domain.Employee;

public sealed class DeleteTests
{
    [Fact]
    public void GivenEmployee_WhenDeleting_ThenIsDeletedIsTrue()
    {
        // Arrange
        var employee = EmployeeFactory.Create();

        // Act
        employee.Delete();

        // Assert
        employee.IsDeleted.ShouldBeTrue();
    }
}

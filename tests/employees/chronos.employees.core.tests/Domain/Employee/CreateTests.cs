using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.employees.core.tests.Domain.Employee;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidParameters_WhenCreatingEmployee_ThenEmployeeIsCreated()
    {
        // Arrange
        var fullName = FullNameFactory.Create();
        var email = EmailFactory.Create();

        // Act
        var employee = chronos.employees.core.Domain.Employee.Create(fullName, email);

        // Assert
        employee.ShouldNotBeNull();
        employee.Id.Value.ShouldNotBe(Ulid.Empty);
        employee.FullName.ShouldBe(fullName);
        employee.Email.ShouldBe(email);
        employee.SupervisorId.ShouldBeNull();
    }
}

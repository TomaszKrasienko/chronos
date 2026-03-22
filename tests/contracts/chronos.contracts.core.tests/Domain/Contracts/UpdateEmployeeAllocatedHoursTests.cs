using chronos.contracts.core.Domain.Identifiers;
using chronos.shared.kernel.Exceptions;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.Contracts;

public sealed class UpdateEmployeeAllocatedHoursTests
{
    [Fact]
    public void GivenExistingEmployee_WhenUpdatingAllocatedHours_ThenHoursAreUpdated()
    {
        // Arrange
        var contract = ContractFactory.CreateWithEmployee(allocatedHours: 160);
        var contractEmployeeId = contract.Employees.First().Id;
        var newAllocatedHours = 200;

        // Act
        contract.UpdateEmployeeAllocatedHours(contractEmployeeId, newAllocatedHours);

        // Assert
        contract.Employees.First().AllocatedHours.ShouldBe(newAllocatedHours);
    }

    [Fact]
    public void GivenNonExistingEmployee_WhenUpdatingAllocatedHours_ThenThrowsDomainExceptionWithCode_employee_assignment_not_found()
    {
        // Arrange
        var contract = ContractFactory.Create();
        var nonExistingId = new ContractEmployeeId(Ulid.NewUlid());

        // Act
        var exception = Should.Throw<DomainException>(
            () => contract.UpdateEmployeeAllocatedHours(nonExistingId, 160));

        // Assert
        exception.Code.ShouldBe("employee_assignment_not_found");
    }

    [Fact]
    public void GivenZeroAllocatedHours_WhenUpdatingAllocatedHours_ThenThrowsDomainExceptionWithCode_allocated_hours_must_be_greater_than_zero()
    {
        // Arrange
        var contract = ContractFactory.CreateWithEmployee();
        var contractEmployeeId = contract.Employees.First().Id;

        // Act
        var exception = Should.Throw<DomainException>(
            () => contract.UpdateEmployeeAllocatedHours(contractEmployeeId, 0));

        // Assert
        exception.Code.ShouldBe("allocated_hours_must_be_greater_than_zero");
    }
}

using chronos.contracts.core.Domain.Events;
using chronos.contracts.core.Domain.Identifiers;
using chronos.shared.kernel.Exceptions;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.Contracts;

public sealed class UpdateEmployeeAssignmentPeriodTests
{
    [Fact]
    public void GivenValidAssignmentPeriod_WhenUpdating_ThenAssignmentPeriodIsUpdated()
    {
        // Arrange
        var contract = ContractFactory.CreateWithEmployee();
        var contractEmployeeId = contract.Employees.First().Id;
        var newFrom = new DateOnly(2025, 1, 1);
        var newTo = new DateOnly(2025, 12, 31);
        var newAssignmentPeriod = AssignmentPeriodFactory.Create(from: newFrom, to: newTo);

        // Act
        contract.UpdateEmployeeAssignmentPeriod(contractEmployeeId, newAssignmentPeriod);

        // Assert
        var employee = contract.Employees.First(x => x.Id == contractEmployeeId);
        employee.AssignmentPeriod.From.ShouldBe(newFrom);
        employee.AssignmentPeriod.To.ShouldBe(newTo);
    }

    [Fact]
    public void GivenValidAssignmentPeriod_WhenUpdating_ThenDomainEventIsRaised()
    {
        // Arrange
        var contract = ContractFactory.CreateWithEmployee();
        contract.ClearDomainEvents();
        var contractEmployeeId = contract.Employees.First().Id;
        var newAssignmentPeriod = AssignmentPeriodFactory.Create(
            from: new DateOnly(2025, 1, 1),
            to: new DateOnly(2025, 12, 31));

        // Act
        contract.UpdateEmployeeAssignmentPeriod(contractEmployeeId, newAssignmentPeriod);

        // Assert
        var domainEvent = contract.DomainEvents
            .OfType<EmployeeAssignmentPeriodUpdatedEvent>()
            .ShouldHaveSingleItem();
        domainEvent.ContractId.ShouldBe(contract.Id);
        domainEvent.ContractEmployeeId.ShouldBe(contractEmployeeId);
        domainEvent.From.ShouldBe(newAssignmentPeriod.From);
        domainEvent.To.ShouldBe(newAssignmentPeriod.To);
    }

    [Fact]
    public void GivenNonExistentEmployee_WhenUpdating_ThenThrowsDomainExceptionWithCode_employee_assignment_not_found()
    {
        // Arrange
        var contract = ContractFactory.Create();
        var nonExistentEmployeeId = ContractEmployeeId.New();
        var assignmentPeriod = AssignmentPeriodFactory.Create();

        // Act
        var exception = Should.Throw<DomainException>(
            () => contract.UpdateEmployeeAssignmentPeriod(nonExistentEmployeeId, assignmentPeriod));

        // Assert
        exception.Code.ShouldBe("employee_assignment_not_found");
    }
}

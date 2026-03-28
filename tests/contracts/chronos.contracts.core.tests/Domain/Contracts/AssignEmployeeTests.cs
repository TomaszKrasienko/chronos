using chronos.contracts.core.Domain;
using chronos.shared.kernel.Exceptions;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.Contracts;

public sealed class AssignEmployeeTests
{
    [Fact]
    public void GivenValidParameters_WhenAssigningEmployee_ThenEmployeeIsAdded()
    {
        // Arrange
        var contract = ContractFactory.Create();
        var employeeId = Ulid.NewUlid();
        var assignmentPeriod = AssignmentPeriodFactory.Create();
        var allocatedHours = 160;

        // Act
        contract.AssignEmployee(
            employeeId,
            assignmentPeriod,
            allocatedHours);

        // Assert
        contract.Employees.ShouldHaveSingleItem();
        var employee = contract.Employees.First();
        employee.EmployeeId.ShouldBe(employeeId);
        employee.AssignmentPeriod.ShouldBe(assignmentPeriod);
        employee.AllocatedHours.ShouldBe(allocatedHours);
    }

    [Fact]
    public void GivenEmployeeAlreadyAssigned_WhenAssigningEmployee_ThenThrowsDomainExceptionWithCode_employee_already_assigned_to_contract()
    {
        // Arrange
        var employeeId = Ulid.NewUlid();
        var contract = ContractFactory.CreateWithEmployee(employeeId: employeeId);

        // Act
        var exception = Should.Throw<DomainException>(
            () => contract.AssignEmployee(employeeId, AssignmentPeriodFactory.Create(), 160));

        // Assert
        exception.Code.ShouldBe("employee_already_assigned_to_contract");
    }

    [Fact]
    public void GivenZeroAllocatedHours_WhenAssigningEmployee_ThenThrowsDomainExceptionWithCode_allocated_hours_must_be_greater_than_zero()
    {
        // Arrange
        var contract = ContractFactory.Create();

        // Act
        var exception = Should.Throw<DomainException>(
            () => contract.AssignEmployee(Ulid.NewUlid(), AssignmentPeriodFactory.Create(), 0));

        // Assert
        exception.Code.ShouldBe("allocated_hours_must_be_greater_than_zero");
    }

    [Fact]
    public void GivenAssignmentStartBeforeContractDate_WhenAssigningEmployee_ThenThrowsDomainExceptionWithCode_employee_assignment_cannot_start_before_contract()
    {
        // Arrange
        var contractPeriod = ContractPeriodFactory.Create(assignmentDate: new DateOnly(2024, 6, 1));
        var contract = ContractFactory.Create(contractPeriod: contractPeriod);
        var assignmentPeriod = AssignmentPeriodFactory.Create(from: new DateOnly(2024, 1, 1));

        // Act
        var exception = Should.Throw<DomainException>(
            () => contract.AssignEmployee(Ulid.NewUlid(), assignmentPeriod, 160));

        // Assert
        exception.Code.ShouldBe("employee_assignment_cannot_start_before_contract");
    }
}

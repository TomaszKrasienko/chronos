using chronos.contracts.core.Domain;
using chronos.contracts.core.Domain.ValueObjects;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.DTOs.Mappers.ContractExtensions;

public sealed class ToDtoTests
{
    [Fact]
    public void GivenContract_WhenMappingToDto_ThenContractFieldsAreMapped()
    {
        // Arrange
        var companyDetails = CompanyDetails.Create(CompanyName);
        var assignmentDate = new DateOnly(2024, 1, 1);
        var closingDate = new DateOnly(2024, 12, 31);
        var contractPeriod = ContractPeriod.Create(assignmentDate);
        var contract = ContractFactory.Create(
            companyDetails: companyDetails,
            contractPeriod: contractPeriod);
        contract.CloseContract(closingDate);

        // Act
        var result = contract.ToDto();

        // Assert
        result.Id.ShouldBe(contract.Id.Value.ToString());
        result.CompanyName.ShouldBe(CompanyName);
        result.ContractPeriod.AssignmentDate.ShouldBe(assignmentDate);
        result.ContractPeriod.ClosingDate.ShouldBe(closingDate);
        result.Employees.ShouldBeEmpty();
    }

    [Fact]
    public void GivenContractWithEmployee_WhenMappingToDto_ThenEmployeeFieldsAreMapped()
    {
        // Arrange
        var employeeId = Ulid.NewUlid();
        var assignmentPeriod = AssignmentPeriod.Create(
            new DateOnly(2024, 1, 1),
            new DateOnly(2024, 12, 31));
        var allocatedHours = 160;
        var contract = ContractFactory.CreateWithEmployee(
            employeeId: employeeId,
            assignmentPeriod: assignmentPeriod,
            allocatedHours: allocatedHours);

        // Act
        var result = contract.ToDto();

        // Assert
        result.Employees.Count.ShouldBe(1);
        var employeeDto = result.Employees[0];
        employeeDto.Id.ShouldBe(contract.Employees[0].Id.Value.ToString());
        employeeDto.EmployeeId.ShouldBe(employeeId.ToString());
        employeeDto.AssignmentPeriod.From.ShouldBe(new DateOnly(2024, 1, 1));
        employeeDto.AssignmentPeriod.To.ShouldBe(new DateOnly(2024, 12, 31));
        employeeDto.AllocatedHours.ShouldBe(allocatedHours);
    }

    private const string CompanyName = "Test Company";
}

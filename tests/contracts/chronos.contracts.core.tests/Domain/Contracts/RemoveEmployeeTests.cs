using chronos.contracts.core.Domain.Identifiers;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.Contracts;

public sealed class RemoveEmployeeTests
{
    [Fact]
    public void GivenExistingEmployee_WhenRemovingEmployee_ThenEmployeeIsRemoved()
    {
        // Arrange
        var contract = ContractFactory.CreateWithEmployee();
        var contractEmployeeId = contract.Employees.First().Id;

        // Act
        contract.RemoveEmployee(contractEmployeeId);

        // Assert
        contract.Employees.ShouldBeEmpty();
    }

}

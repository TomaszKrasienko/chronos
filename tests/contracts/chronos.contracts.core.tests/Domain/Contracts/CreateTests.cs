using chronos.contracts.core.Domain;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.Contracts;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidParameters_WhenCreatingContract_ThenContractIsCreated()
    {
        // Arrange
        var companyDetails = CompanyDetailsFactory.Create();
        var contractPeriod = ContractPeriodFactory.Create();

        // Act
        var contract = Contract.Create(companyDetails, contractPeriod);

        // Assert
        contract.ShouldNotBeNull();
        contract.Id.Value.ShouldNotBe(Ulid.Empty);
        contract.CompanyDetails.ShouldBe(companyDetails);
        contract.ContractPeriod.ShouldBe(contractPeriod);
        contract.Employees.ShouldBeEmpty();
    }
}

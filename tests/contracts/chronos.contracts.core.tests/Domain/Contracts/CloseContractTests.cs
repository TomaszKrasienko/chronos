using chronos.shared.kernel.Exceptions;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.Contracts;

public sealed class CloseContractTests
{
    [Fact]
    public void GivenValidClosingDate_WhenClosingContract_ThenContractIsClosed()
    {
        // Arrange
        var assignmentDate = new DateOnly(2024, 1, 1);
        var contract = ContractFactory.Create(
            contractPeriod: ContractPeriodFactory.Create(assignmentDate: assignmentDate));
        var closingDate = new DateOnly(2024, 12, 31);

        // Act
        contract.CloseContract(closingDate);

        // Assert
        contract.ContractPeriod.ClosingDate.ShouldBe(closingDate);
    }

    [Fact]
    public void GivenClosingDateBeforeAssignment_WhenClosingContract_ThenThrowsDomainExceptionWithCode_closing_date_cannot_be_before_assignment_date()
    {
        // Arrange
        var assignmentDate = new DateOnly(2024, 6, 1);
        var contract = ContractFactory.Create(
            contractPeriod: ContractPeriodFactory.Create(assignmentDate: assignmentDate));
        var closingDate = new DateOnly(2024, 1, 1);

        // Act
        var exception = Should.Throw<DomainException>(
            () => contract.CloseContract(closingDate));

        // Assert
        exception.Code.ShouldBe("closing_date_cannot_be_before_assignment_date");
    }
}

using chronos.shared.kernel.Exceptions;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.ValueObjects.ContractPeriod;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidAssignmentDate_WhenCreating_ThenContractPeriodIsCreated()
    {
        // Arrange
        var assignmentDate = new DateOnly(2024, 1, 1);

        // Act
        var period = contracts.core.Domain.ValueObjects.ContractPeriod.Create(assignmentDate);

        // Assert
        period.AssignmentDate.ShouldBe(assignmentDate);
        period.ClosingDate.ShouldBeNull();
    }

    [Fact]
    public void GivenValidDates_WhenCreatingWithClosingDate_ThenContractPeriodIsCreated()
    {
        // Arrange
        var assignmentDate = new DateOnly(2024, 1, 1);
        var closingDate = new DateOnly(2024, 12, 31);

        // Act
        var period = contracts.core.Domain.ValueObjects.ContractPeriod.Create(assignmentDate, closingDate);

        // Assert
        period.AssignmentDate.ShouldBe(assignmentDate);
        period.ClosingDate.ShouldBe(closingDate);
    }

    [Fact]
    public void GivenClosingDateBeforeAssignment_WhenCreating_ThenThrowsDomainExceptionWithCode_closing_date_cannot_be_before_assignment_date()
    {
        // Arrange
        var assignmentDate = new DateOnly(2024, 12, 31);
        var closingDate = new DateOnly(2024, 1, 1);

        // Act
        var exception = Should.Throw<DomainException>(
            () => contracts.core.Domain.ValueObjects.ContractPeriod.Create(assignmentDate, closingDate));

        // Assert
        exception.Code.ShouldBe("closing_date_cannot_be_before_assignment_date");
    }
}

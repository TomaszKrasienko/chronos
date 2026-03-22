using chronos.shared.kernel.Exceptions;
using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.ValueObjects.ContractPeriod;

public sealed class CloseTests
{
    [Fact]
    public void GivenValidClosingDate_WhenClosing_ThenReturnsNewPeriodWithClosingDate()
    {
        // Arrange
        var assignmentDate = new DateOnly(2024, 1, 1);
        var period = ContractPeriodFactory.Create(assignmentDate: assignmentDate);
        var closingDate = new DateOnly(2024, 12, 31);

        // Act
        var closedPeriod = period.Close(closingDate);

        // Assert
        closedPeriod.AssignmentDate.ShouldBe(assignmentDate);
        closedPeriod.ClosingDate.ShouldBe(closingDate);
    }

    [Fact]
    public void GivenClosingDateBeforeAssignment_WhenClosing_ThenThrowsDomainExceptionWithCode_closing_date_cannot_be_before_assignment_date()
    {
        // Arrange
        var assignmentDate = new DateOnly(2024, 6, 1);
        var period = ContractPeriodFactory.Create(assignmentDate: assignmentDate);
        var closingDate = new DateOnly(2024, 1, 1);

        // Act
        var exception = Should.Throw<DomainException>(
            () => period.Close(closingDate));

        // Assert
        exception.Code.ShouldBe("closing_date_cannot_be_before_assignment_date");
    }
}

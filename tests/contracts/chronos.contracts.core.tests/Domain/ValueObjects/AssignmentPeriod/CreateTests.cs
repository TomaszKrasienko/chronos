using chronos.contracts.core.Domain.ValueObjects;
using chronos.shared.kernel.Exceptions;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.ValueObjects.AssignmentPeriod;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidDates_WhenCreating_ThenAssignmentPeriodIsCreated()
    {
        // Arrange
        var from = new DateOnly(2024, 1, 1);
        var to = new DateOnly(2024, 12, 31);

        // Act
        var period = contracts.core.Domain.ValueObjects.AssignmentPeriod.Create(from, to);

        // Assert
        period.From.ShouldBe(from);
        period.To.ShouldBe(to);
    }

    [Fact]
    public void GivenSameDates_WhenCreating_ThenAssignmentPeriodIsCreated()
    {
        // Arrange
        var date = new DateOnly(2024, 6, 15);

        // Act
        var period = contracts.core.Domain.ValueObjects.AssignmentPeriod.Create(date, date);

        // Assert
        period.From.ShouldBe(date);
        period.To.ShouldBe(date);
    }

    [Fact]
    public void GivenEndDateBeforeStartDate_WhenCreating_ThenThrowsDomainExceptionWithCode_end_date_cannot_be_before_start_date()
    {
        // Arrange
        var from = new DateOnly(2024, 12, 31);
        var to = new DateOnly(2024, 1, 1);

        // Act
        var exception = Should.Throw<DomainException>(
            () => contracts.core.Domain.ValueObjects.AssignmentPeriod.Create(from, to));

        // Assert
        exception.Code.ShouldBe("end_date_cannot_be_before_start_date");
    }
}

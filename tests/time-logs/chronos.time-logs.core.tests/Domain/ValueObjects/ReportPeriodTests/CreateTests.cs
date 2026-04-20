using chronos.shared.kernel.Exceptions;
using Shouldly;

namespace chronos.time_logs.core.tests.Domain.ValueObjects.ReportPeriodTests;

public sealed class CreateTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(12)]
    public void GivenValidMonth_WhenCreating_ThenReportPeriodIsCreated(int month)
    {
        // Arrange
        var year = 2026;

        // Act
        var result = time_logs.core.Domain.ValueObjects.ReportPeriod.Create(month, year);

        // Assert
        result.Month.ShouldBe(month);
        result.Year.ShouldBe(year);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(13)]
    [InlineData(100)]
    public void GivenInvalidMonth_WhenCreating_ThenThrowsDomainExceptionWithCode_invalid_month(int month)
    {
        // Arrange
        var year = 2026;

        // Act
        var act = () => time_logs.core.Domain.ValueObjects.ReportPeriod.Create(month, year);

        // Assert
        var exception = act.ShouldThrow<DomainException>();
        exception.Code.ShouldBe("invalid_month");
    }
}

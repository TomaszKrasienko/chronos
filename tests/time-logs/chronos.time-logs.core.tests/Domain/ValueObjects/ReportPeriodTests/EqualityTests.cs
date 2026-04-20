using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.time_logs.core.tests.Domain.ValueObjects.ReportPeriodTests;

public sealed class EqualityTests
{
    [Fact]
    public void GivenSameMonthAndYear_WhenComparing_ThenAreEqual()
    {
        // Arrange
        var period1 = ReportPeriodFactory.Create(month: 4, year: 2026);
        var period2 = ReportPeriodFactory.Create(month: 4, year: 2026);

        // Act & Assert
        period1.ShouldBe(period2);
    }

    [Fact]
    public void GivenDifferentMonth_WhenComparing_ThenAreNotEqual()
    {
        // Arrange
        var period1 = ReportPeriodFactory.Create(month: 4, year: 2026);
        var period2 = ReportPeriodFactory.Create(month: 5, year: 2026);

        // Act & Assert
        period1.ShouldNotBe(period2);
    }

    [Fact]
    public void GivenDifferentYear_WhenComparing_ThenAreNotEqual()
    {
        // Arrange
        var period1 = ReportPeriodFactory.Create(month: 4, year: 2026);
        var period2 = ReportPeriodFactory.Create(month: 4, year: 2027);

        // Act & Assert
        period1.ShouldNotBe(period2);
    }
}

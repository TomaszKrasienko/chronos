using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.ValueObjects.AssignmentPeriod;

public sealed class EqualityTests
{
    [Fact]
    public void GivenSameValues_WhenComparing_ThenAreEqual()
    {
        // Arrange
        var from = new DateOnly(2024, 1, 1);
        var to = new DateOnly(2024, 12, 31);
        var period1 = contracts.core.Domain.ValueObjects.AssignmentPeriod.Create(from, to);
        var period2 = contracts.core.Domain.ValueObjects.AssignmentPeriod.Create(from, to);

        // Assert
        period1.ShouldBe(period2);
    }

    [Fact]
    public void GivenDifferentValues_WhenComparing_ThenAreNotEqual()
    {
        // Arrange
        var period1 = AssignmentPeriodFactory.Create(
            from: new DateOnly(2024, 1, 1),
            to: new DateOnly(2024, 6, 30));
        var period2 = AssignmentPeriodFactory.Create(
            from: new DateOnly(2024, 7, 1),
            to: new DateOnly(2024, 12, 31));

        // Assert
        period1.ShouldNotBe(period2);
    }
}

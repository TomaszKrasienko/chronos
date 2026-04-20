using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.time_logs.core.tests.Domain.ValueObjects.LoggedTimeTests;

public sealed class EqualityTests
{
    [Fact]
    public void GivenSameTime_WhenComparing_ThenAreEqual()
    {
        // Arrange
        var time = TimeSpan.FromHours(8);
        var loggedTime1 = LoggedTimeFactory.Create(time);
        var loggedTime2 = LoggedTimeFactory.Create(time);

        // Act & Assert
        loggedTime1.ShouldBe(loggedTime2);
    }

    [Fact]
    public void GivenDifferentTime_WhenComparing_ThenAreNotEqual()
    {
        // Arrange
        var loggedTime1 = LoggedTimeFactory.Create(TimeSpan.FromHours(8));
        var loggedTime2 = LoggedTimeFactory.Create(TimeSpan.FromHours(4));

        // Act & Assert
        loggedTime1.ShouldNotBe(loggedTime2);
    }
}

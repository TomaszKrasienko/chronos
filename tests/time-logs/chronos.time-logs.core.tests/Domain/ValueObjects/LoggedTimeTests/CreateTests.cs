using chronos.shared.kernel.Exceptions;
using chronos.time_logs.core.Domain.ValueObjects;
using Shouldly;

namespace chronos.time_logs.core.tests.Domain.ValueObjects.LoggedTimeTests;

public sealed class CreateTests
{
    [Fact]
    public void GivenPositiveTime_WhenCreating_ThenLoggedTimeIsCreated()
    {
        // Arrange
        var time = TimeSpan.FromHours(8);

        // Act
        var result = LoggedTime.Create(time);

        // Assert
        result.Value.ShouldBe(time);
    }

    [Fact]
    public void GivenZeroTime_WhenCreating_ThenThrowsDomainExceptionWithCode_time_must_be_positive()
    {
        // Arrange
        var time = TimeSpan.Zero;

        // Act
        var act = () => LoggedTime.Create(time);

        // Assert
        var exception = act.ShouldThrow<DomainException>();
        exception.Code.ShouldBe("time_must_be_positive");
    }

    [Fact]
    public void GivenNegativeTime_WhenCreating_ThenThrowsDomainExceptionWithCode_time_must_be_positive()
    {
        // Arrange
        var time = TimeSpan.FromHours(-1);

        // Act
        var act = () => LoggedTime.Create(time);

        // Assert
        var exception = act.ShouldThrow<DomainException>();
        exception.Code.ShouldBe("time_must_be_positive");
    }
}

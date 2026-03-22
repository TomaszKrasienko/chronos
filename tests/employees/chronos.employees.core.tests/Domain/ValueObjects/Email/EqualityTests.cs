using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.employees.core.tests.Domain.ValueObjects.Email;

public sealed class EqualityTests
{
    [Fact]
    public void GivenSameValue_WhenComparing_ThenAreEqual()
    {
        // Arrange
        var email1 = EmailFactory.Create(value: "test@example.com");
        var email2 = EmailFactory.Create(value: "test@example.com");

        // Assert
        email1.ShouldBe(email2);
    }

    [Fact]
    public void GivenDifferentValue_WhenComparing_ThenAreNotEqual()
    {
        // Arrange
        var email1 = EmailFactory.Create(value: "test@example.com");
        var email2 = EmailFactory.Create(value: "other@example.com");

        // Assert
        email1.ShouldNotBe(email2);
    }
}

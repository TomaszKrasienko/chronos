using chronos.tests.shared.Factories;
using Shouldly;

namespace chronos.employees.core.tests.Domain.ValueObjects.FullName;

public sealed class EqualityTests
{
    [Fact]
    public void GivenSameValues_WhenComparing_ThenAreEqual()
    {
        // Arrange
        var fullName1 = FullNameFactory.Create(firstName: "John", lastName: "Doe");
        var fullName2 = FullNameFactory.Create(firstName: "John", lastName: "Doe");

        // Assert
        fullName1.ShouldBe(fullName2);
    }

    [Fact]
    public void GivenDifferentFirstName_WhenComparing_ThenAreNotEqual()
    {
        // Arrange
        var fullName1 = FullNameFactory.Create(firstName: "John", lastName: "Doe");
        var fullName2 = FullNameFactory.Create(firstName: "Jane", lastName: "Doe");

        // Assert
        fullName1.ShouldNotBe(fullName2);
    }

    [Fact]
    public void GivenDifferentLastName_WhenComparing_ThenAreNotEqual()
    {
        // Arrange
        var fullName1 = FullNameFactory.Create(firstName: "John", lastName: "Doe");
        var fullName2 = FullNameFactory.Create(firstName: "John", lastName: "Smith");

        // Assert
        fullName1.ShouldNotBe(fullName2);
    }
}

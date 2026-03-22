using chronos.employees.core.Domain.ValueObjects;
using chronos.shared.kernel.Exceptions;
using Shouldly;

namespace chronos.employees.core.tests.Domain.ValueObjects.FullName;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidNames_WhenCreating_ThenFullNameIsCreated()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var fullName = chronos.employees.core.Domain.ValueObjects.FullName.Create(firstName, lastName);

        // Assert
        fullName.ShouldNotBeNull();
        fullName.FirstName.ShouldBe(firstName);
        fullName.LastName.ShouldBe(lastName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenInvalidFirstName_WhenCreating_ThenThrowsDomainExceptionWithCode_first_name_cannot_be_empty(string? firstName)
    {
        // Arrange
        var lastName = "Doe";

        // Act
        var exception = Should.Throw<DomainException>(
            () => chronos.employees.core.Domain.ValueObjects.FullName.Create(firstName!, lastName));

        // Assert
        exception.Code.ShouldBe("first_name_cannot_be_empty");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenInvalidLastName_WhenCreating_ThenThrowsDomainExceptionWithCode_last_name_cannot_be_empty(string? lastName)
    {
        // Arrange
        var firstName = "John";

        // Act
        var exception = Should.Throw<DomainException>(
            () => chronos.employees.core.Domain.ValueObjects.FullName.Create(firstName, lastName!));

        // Assert
        exception.Code.ShouldBe("last_name_cannot_be_empty");
    }
}

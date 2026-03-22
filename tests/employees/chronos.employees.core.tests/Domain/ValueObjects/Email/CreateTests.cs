using chronos.employees.core.Domain.ValueObjects;
using chronos.shared.kernel.Exceptions;
using Shouldly;

namespace chronos.employees.core.tests.Domain.ValueObjects.Email;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidEmail_WhenCreating_ThenEmailIsCreated()
    {
        // Arrange
        var value = "test@example.com";

        // Act
        var email = chronos.employees.core.Domain.ValueObjects.Email.Create(value);

        // Assert
        email.ShouldNotBeNull();
        email.Value.ShouldBe(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenInvalidEmail_WhenCreating_ThenThrowsDomainExceptionWithCode_email_cannot_be_empty(string? value)
    {
        // Act
        var exception = Should.Throw<DomainException>(
            () => chronos.employees.core.Domain.ValueObjects.Email.Create(value!));

        // Assert
        exception.Code.ShouldBe("email_cannot_be_empty");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("user@")]
    public void GivenInvalidFormat_WhenCreating_ThenThrowsDomainExceptionWithCode_email_must_be_valid_format(string value)
    {
        // Act
        var exception = Should.Throw<DomainException>(
            () => chronos.employees.core.Domain.ValueObjects.Email.Create(value));

        // Assert
        exception.Code.ShouldBe("email_must_be_valid_format");
    }
}

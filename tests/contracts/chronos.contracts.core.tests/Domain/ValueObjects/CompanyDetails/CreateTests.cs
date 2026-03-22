using chronos.shared.kernel.Exceptions;
using Shouldly;

namespace chronos.contracts.core.tests.Domain.ValueObjects.CompanyDetails;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidName_WhenCreating_ThenCompanyDetailsIsCreated()
    {
        // Arrange
        var name = "Test Company";

        // Act
        var details = contracts.core.Domain.ValueObjects.CompanyDetails.Create(name);

        // Assert
        details.Name.ShouldBe(name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenInvalidName_WhenCreating_ThenThrowsDomainExceptionWithCode_company_name_cannot_be_empty(string? name)
    {
        // Act
        var exception = Should.Throw<DomainException>(
            () => contracts.core.Domain.ValueObjects.CompanyDetails.Create(name!));

        // Assert
        exception.Code.ShouldBe("company_name_cannot_be_empty");
    }
}

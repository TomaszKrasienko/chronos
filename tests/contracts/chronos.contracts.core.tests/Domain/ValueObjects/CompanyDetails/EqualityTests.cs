using Shouldly;

namespace chronos.contracts.core.tests.Domain.ValueObjects.CompanyDetails;

public sealed class EqualityTests
{
    [Fact]
    public void GivenSameName_WhenComparing_ThenAreEqual()
    {
        // Arrange
        var details1 = contracts.core.Domain.ValueObjects.CompanyDetails.Create("Test Company");
        var details2 = contracts.core.Domain.ValueObjects.CompanyDetails.Create("Test Company");

        // Assert
        details1.ShouldBe(details2);
    }

    [Fact]
    public void GivenDifferentNames_WhenComparing_ThenAreNotEqual()
    {
        // Arrange
        var details1 = contracts.core.Domain.ValueObjects.CompanyDetails.Create("Company A");
        var details2 = contracts.core.Domain.ValueObjects.CompanyDetails.Create("Company B");

        // Assert
        details1.ShouldNotBe(details2);
    }
}

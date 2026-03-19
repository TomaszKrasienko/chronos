using chronos.contracts.core.Domain.ValueObjects;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="CompanyDetails"/> instances in tests.
/// </summary>
public static class CompanyDetailsFactory
{
    private const string DefaultName = "Test Company";

    public static CompanyDetails Create(string? name = null)
        => CompanyDetails.Create(name ?? DefaultName);
}

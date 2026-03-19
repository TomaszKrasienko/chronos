using chronos.contracts.core.Domain.Rules;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.ValueObjects;

/// <summary>
/// Value object representing company details for a contract.
/// </summary>
public sealed class CompanyDetails : ValueObject
{
    /// <summary>
    /// Gets the company name.
    /// </summary>
    public string Name { get; }

    private CompanyDetails(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates a new company details instance.
    /// </summary>
    /// <param name="name">The company name.</param>
    public static CompanyDetails Create(string name)
    {
        CheckRule(new CompanyNameCannotBeEmptyRule(name));
        return new CompanyDetails(name);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}

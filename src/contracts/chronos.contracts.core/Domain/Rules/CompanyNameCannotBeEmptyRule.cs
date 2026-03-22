using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Rules;

/// <summary>
/// Rule that validates company name cannot be empty or whitespace.
/// </summary>
internal sealed class CompanyNameCannotBeEmptyRule(string name) : IBusinessRule
{
    public string Code => "company_name_cannot_be_empty";
    public bool IsBroken() => string.IsNullOrWhiteSpace(name);
}

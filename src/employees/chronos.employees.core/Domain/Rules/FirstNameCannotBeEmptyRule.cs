using chronos.shared.kernel;

namespace chronos.employees.core.Domain.Rules;

/// <summary>
/// Rule that validates first name cannot be empty or whitespace.
/// </summary>
internal sealed class FirstNameCannotBeEmptyRule(string firstName) : IBusinessRule
{
    public string Code => "first_name_cannot_be_empty";
    public bool IsBroken() => string.IsNullOrWhiteSpace(firstName);
}

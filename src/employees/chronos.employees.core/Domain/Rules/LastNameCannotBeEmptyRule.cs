using chronos.shared.kernel;

namespace chronos.employees.core.Domain.Rules;

/// <summary>
/// Rule that validates last name cannot be empty or whitespace.
/// </summary>
internal sealed class LastNameCannotBeEmptyRule(string lastName) : IBusinessRule
{
    public string Code => "last_name_cannot_be_empty";
    public bool IsBroken() => string.IsNullOrWhiteSpace(lastName);
}

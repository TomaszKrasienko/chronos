using chronos.shared.kernel;

namespace chronos.employees.core.Domain.Rules;

/// <summary>
/// Rule that validates email cannot be empty or whitespace.
/// </summary>
internal sealed class EmailCannotBeEmptyRule(string email) : IBusinessRule
{
    public string Code => "email_cannot_be_empty";
    public bool IsBroken() => string.IsNullOrWhiteSpace(email);
}

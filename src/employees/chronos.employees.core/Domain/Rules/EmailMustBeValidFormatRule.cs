using System.Net.Mail;
using chronos.shared.kernel;

namespace chronos.employees.core.Domain.Rules;

/// <summary>
/// Rule that validates email must have a valid format.
/// </summary>
internal sealed class EmailMustBeValidFormatRule(string email) : IBusinessRule
{
    public string Code => "email_must_be_valid_format";
    public bool IsBroken() => !MailAddress.TryCreate(email, out _);
}

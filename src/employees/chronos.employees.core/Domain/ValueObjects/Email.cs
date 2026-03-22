using chronos.employees.core.Domain.Rules;
using chronos.shared.kernel;

namespace chronos.employees.core.Domain.ValueObjects;

/// <summary>
/// Value object representing an employee's email address.
/// </summary>
public sealed class Email : ValueObject
{
    /// <summary>
    /// Gets the email address value.
    /// </summary>
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new email instance.
    /// </summary>
    /// <param name="value">The email address.</param>
    public static Email Create(string value)
    {
        CheckRule(new EmailCannotBeEmptyRule(value));
        CheckRule(new EmailMustBeValidFormatRule(value));
        return new Email(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

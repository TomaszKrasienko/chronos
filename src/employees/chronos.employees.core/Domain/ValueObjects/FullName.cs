using chronos.employees.core.Domain.Rules;
using chronos.shared.kernel;

namespace chronos.employees.core.Domain.ValueObjects;

/// <summary>
/// Value object representing an employee's full name.
/// </summary>
public sealed class FullName : ValueObject
{
    /// <summary>
    /// Gets the first name.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the last name.
    /// </summary>
    public string LastName { get; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    /// <summary>
    /// Creates a new full name instance.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    public static FullName Create(string firstName, string lastName)
    {
        CheckRule(new FirstNameCannotBeEmptyRule(firstName));
        CheckRule(new LastNameCannotBeEmptyRule(lastName));
        return new FullName(firstName, lastName);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}

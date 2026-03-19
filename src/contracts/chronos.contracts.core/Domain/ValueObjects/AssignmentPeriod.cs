using chronos.contracts.core.Domain.Rules;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.ValueObjects;

/// <summary>
/// Value object representing an employee assignment period within a contract.
/// </summary>
public sealed class AssignmentPeriod : ValueObject
{
    /// <summary>
    /// Gets the start date of the assignment.
    /// </summary>
    public DateOnly From { get; }

    /// <summary>
    /// Gets the end date of the assignment.
    /// </summary>
    public DateOnly To { get; }

    private AssignmentPeriod(DateOnly from, DateOnly to)
    {
        From = from;
        To = to;
    }

    /// <summary>
    /// Creates a new assignment period.
    /// </summary>
    /// <param name="from">The start date.</param>
    /// <param name="to">The end date.</param>
    public static AssignmentPeriod Create(DateOnly from, DateOnly to)
    {
        CheckRule(new EndDateCannotBeBeforeStartDateRule(from, to));
        return new AssignmentPeriod(from, to);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return From;
        yield return To;
    }
}

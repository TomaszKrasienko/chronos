using chronos.contracts.core.Domain.Rules;
using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.ValueObjects;

/// <summary>
/// Value object representing the contract period with assignment and closing dates.
/// </summary>
public sealed class ContractPeriod : ValueObject
{
    /// <summary>
    /// Gets the date when the contract was assigned.
    /// </summary>
    public DateOnly AssignmentDate { get; }

    /// <summary>
    /// Gets the date when the contract was closed, or null if still active.
    /// </summary>
    public DateOnly? ClosingDate { get; }

    private ContractPeriod(DateOnly assignmentDate, DateOnly? closingDate)
    {
        AssignmentDate = assignmentDate;
        ClosingDate = closingDate;
    }

    /// <summary>
    /// Creates a new contract period.
    /// </summary>
    /// <param name="assignmentDate">The assignment date.</param>
    /// <param name="closingDate">The optional closing date.</param>
    public static ContractPeriod Create(DateOnly assignmentDate, DateOnly? closingDate = null)
    {
        CheckRule(new ClosingDateCannotBeBeforeAssignmentDateRule(assignmentDate, closingDate));
        return new ContractPeriod(assignmentDate, closingDate);
    }

    /// <summary>
    /// Creates a new contract period with the specified closing date.
    /// </summary>
    /// <param name="closingDate">The closing date.</param>
    public ContractPeriod Close(DateOnly closingDate)
    {
        CheckRule(new ClosingDateCannotBeBeforeAssignmentDateRule(AssignmentDate, closingDate));
        return new ContractPeriod(AssignmentDate, closingDate);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return AssignmentDate;
        yield return ClosingDate;
    }
}

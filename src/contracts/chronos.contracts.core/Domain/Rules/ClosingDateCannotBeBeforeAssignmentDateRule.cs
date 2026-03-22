using chronos.shared.kernel;

namespace chronos.contracts.core.Domain.Rules;

/// <summary>
/// Rule that validates closing date cannot be before assignment date.
/// </summary>
internal sealed class ClosingDateCannotBeBeforeAssignmentDateRule(
    DateOnly assignmentDate,
    DateOnly? closingDate) : IBusinessRule
{
    public string Code => "closing_date_cannot_be_before_assignment_date";
    public bool IsBroken() => closingDate.HasValue && closingDate.Value < assignmentDate;
}

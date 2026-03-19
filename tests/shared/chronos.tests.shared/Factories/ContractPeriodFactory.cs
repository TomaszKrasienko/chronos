using chronos.contracts.core.Domain.ValueObjects;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="ContractPeriod"/> instances in tests.
/// </summary>
public static class ContractPeriodFactory
{
    private static DateOnly DefaultAssignmentDate => new(2024, 1, 1);

    public static ContractPeriod Create(
        DateOnly? assignmentDate = null,
        DateOnly? closingDate = null)
        => ContractPeriod.Create(
            assignmentDate ?? DefaultAssignmentDate,
            closingDate);
}

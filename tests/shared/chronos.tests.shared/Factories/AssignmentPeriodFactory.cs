using chronos.contracts.core.Domain.ValueObjects;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="AssignmentPeriod"/> instances in tests.
/// </summary>
public static class AssignmentPeriodFactory
{
    private static DateOnly DefaultFrom => new(2024, 1, 1);
    private static DateOnly DefaultTo => new(2024, 12, 31);

    public static AssignmentPeriod Create(
        DateOnly? from = null,
        DateOnly? to = null)
        => AssignmentPeriod.Create(
            from ?? DefaultFrom,
            to ?? DefaultTo);
}

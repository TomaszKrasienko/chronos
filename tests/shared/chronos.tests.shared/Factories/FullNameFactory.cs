using chronos.employees.core.Domain.ValueObjects;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="FullName"/> instances in tests.
/// </summary>
public static class FullNameFactory
{
    private const string DefaultFirstName = "John";
    private const string DefaultLastName = "Doe";

    public static FullName Create(
        string? firstName = null,
        string? lastName = null)
        => FullName.Create(
            firstName ?? DefaultFirstName,
            lastName ?? DefaultLastName);
}

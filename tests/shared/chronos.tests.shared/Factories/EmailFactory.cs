using chronos.employees.core.Domain.ValueObjects;

namespace chronos.tests.shared.Factories;

/// <summary>
/// Factory for creating <see cref="Email"/> instances in tests.
/// </summary>
public static class EmailFactory
{
    private const string DefaultEmail = "test@example.com";

    public static Email Create(string? value = null)
        => Email.Create(value ?? DefaultEmail);
}

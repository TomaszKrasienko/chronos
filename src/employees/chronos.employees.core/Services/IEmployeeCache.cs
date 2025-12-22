using chronos.employees.core.Domain;

namespace chronos.employees.core.Services;

/// <summary>
/// Service for managing employee cache operations
/// </summary>
public interface IEmployeeCache
{
    /// <summary>
    /// Adds an employee to the cache
    /// </summary>
    /// <param name="employee">The employee to cache</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task CreateAsync(
        Ulid id,
        string firstName,
        string lastName,
        string email,
        Ulid? supervisorId,
        CancellationToken cancellationToken);
}

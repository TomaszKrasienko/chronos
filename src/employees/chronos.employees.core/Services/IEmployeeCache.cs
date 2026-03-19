namespace chronos.employees.core.Services;

/// <summary>
/// Service for managing employee cache operations
/// </summary>
public interface IEmployeeCache
{
    /// <summary>
    /// Adds an employee to the cache
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    /// <param name="email">Email address</param>
    /// <param name="supervisorId">Optional supervisor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task CreateAsync(
        Ulid id,
        string firstName,
        string lastName,
        string email,
        Ulid? supervisorId,
        CancellationToken cancellationToken);
}

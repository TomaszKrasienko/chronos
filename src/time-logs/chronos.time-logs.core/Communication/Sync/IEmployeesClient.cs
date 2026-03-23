namespace chronos.time_logs.core.Communication.Sync;

public sealed record EmployeeDto(
    Ulid Id,
    string FirstName,
    string LastName,
    Ulid? SupervisorId);

/// <summary>
/// Client for synchronous communication with the Employees service
/// </summary>
public interface IEmployeesClient
{
    /// <summary>
    /// Checks if an employee exists
    /// </summary>
    /// <param name="employeeId">The unique identifier of the employee</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the employee exists, otherwise false</returns>
    Task<bool> AnyAsync(Ulid employeeId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an employee by ID
    /// </summary>
    /// <param name="employeeId">The unique identifier of the employee</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Employee data or null if not found</returns>
    Task<EmployeeDto?> GetByIdAsync(Ulid employeeId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all subordinates for a supervisor
    /// </summary>
    /// <param name="supervisorId">The unique identifier of the supervisor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of employees reporting to the supervisor</returns>
    Task<IReadOnlyCollection<EmployeeDto>> GetSubordinatesAsync(
        Ulid supervisorId,
        CancellationToken cancellationToken);
}
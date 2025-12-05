namespace chronos.time_loggers.core.Communication.Sync;

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
}
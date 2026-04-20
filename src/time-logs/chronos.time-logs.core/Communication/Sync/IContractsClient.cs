namespace chronos.time_logs.core.Communication.Sync;

/// <summary>
/// Client for communicating with the Contracts service.
/// </summary>
public interface IContractsClient
{
    /// <summary>
    /// Checks if an employee is assigned to a contract.
    /// </summary>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> IsEmployeeAssignedToContractAsync(
        Ulid contractId,
        Ulid employeeId,
        CancellationToken cancellationToken = default);
}

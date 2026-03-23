using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Services;

/// <summary>
/// Service for managing time logs.
/// </summary>
public interface ITimeLogsService
{
    /// <summary>
    /// Creates a new time log.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="hours">The logged hours.</param>
    /// <param name="topic">The topic.</param>
    /// <param name="notes">Optional notes.</param>
    /// <param name="supervisorId">The supervisor identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created time log identifier.</returns>
    Task<TimeLogId> CreateAsync(
        EmployeeId employeeId,
        ContractId contractId,
        TimeSpan hours,
        string topic,
        string? notes,
        EmployeeId supervisorId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Accepts a time log.
    /// </summary>
    /// <param name="timeLogId">The time log identifier.</param>
    /// <param name="supervisorId">The supervisor identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AcceptAsync(
        TimeLogId timeLogId,
        EmployeeId supervisorId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Rejects a time log.
    /// </summary>
    /// <param name="timeLogId">The time log identifier.</param>
    /// <param name="supervisorId">The supervisor identifier.</param>
    /// <param name="reason">The rejection reason.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task RejectAsync(
        TimeLogId timeLogId,
        EmployeeId supervisorId,
        string reason,
        CancellationToken cancellationToken = default);
}

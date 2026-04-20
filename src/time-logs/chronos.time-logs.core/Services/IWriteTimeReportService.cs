using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Services;

/// <summary>
/// Service for writing time reports.
/// </summary>
public interface IWriteTimeReportService
{
    /// <summary>
    /// Adds a new time log to the monthly report.
    /// Creates a new report if one doesn't exist for the current month.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <param name="contractId">The contract identifier.</param>
    /// <param name="time">The logged time.</param>
    /// <param name="topic">The topic.</param>
    /// <param name="notes">Optional notes.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created time log identifier.</returns>
    Task<TimeLogId> AddTimeLogAsync(
        EmployeeId employeeId,
        ContractId contractId,
        TimeSpan time,
        string topic,
        string? notes,
        CancellationToken cancellationToken = default);
}

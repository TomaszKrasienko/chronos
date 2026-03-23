using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Events;

/// <summary>
/// Event raised when a time log is rejected by a supervisor.
/// </summary>
/// <param name="Id">The time log identifier.</param>
/// <param name="EmployeeId">The employee identifier.</param>
/// <param name="ContractId">The contract identifier.</param>
/// <param name="Hours">The logged hours.</param>
/// <param name="Topic">The topic.</param>
/// <param name="Notes">Optional notes.</param>
/// <param name="RejectedBy">The supervisor who rejected.</param>
/// <param name="Reason">The rejection reason.</param>
public sealed record TimeLogRejected(
    TimeLogId Id,
    EmployeeId EmployeeId,
    ContractId ContractId,
    TimeSpan Hours,
    string Topic,
    string? Notes,
    EmployeeId RejectedBy,
    string Reason);

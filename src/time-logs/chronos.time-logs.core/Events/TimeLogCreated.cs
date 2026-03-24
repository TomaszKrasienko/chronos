using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Events;

/// <summary>
/// Event raised when a time log is created and waiting for acceptation.
/// </summary>
/// <param name="Id">The time log identifier.</param>
/// <param name="EmployeeId">The employee identifier.</param>
/// <param name="ContractId">The contract identifier.</param>
/// <param name="Hours">The logged hours.</param>
/// <param name="Topic">The topic.</param>
/// <param name="Notes">Optional notes.</param>
/// <param name="SupervisorId">The supervisor identifier.</param>
public sealed record TimeLogCreated(
    TimeLogId Id,
    EmployeeId EmployeeId,
    ContractId ContractId,
    TimeSpan Hours,
    string Topic,
    string? Notes,
    EmployeeId SupervisorId);

using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.time_logs.core.Domain.Events;

/// <summary>
/// Domain event raised when a time log waiting for acceptation is created.
/// </summary>
/// <param name="TimeLogId">The time log identifier.</param>
/// <param name="ContractId">The contract identifier.</param>
/// <param name="EmployeeId">The employee identifier.</param>
/// <param name="Time">The logged time.</param>
/// <param name="Topic">The topic.</param>
/// <param name="Notes">Optional notes.</param>
/// <param name="SupervisorId">The supervisor identifier.</param>
public sealed record TimeLogWaitingForAcceptationCreatedEvent(
    TimeLogId TimeLogId,
    ContractId ContractId,
    EmployeeId EmployeeId,
    TimeSpan Time,
    string Topic,
    string? Notes,
    EmployeeId SupervisorId) : IDomainEvent;

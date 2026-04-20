using chronos.shared.kernel.Identifiers;
using chronos.shared.messaging;

namespace chronos.time_logs.core.Events;

/// <summary>
/// Integration event raised when a time log waiting for acceptation is created.
/// </summary>
/// <param name="TimeLogId">The time log identifier.</param>
/// <param name="ContractId">The contract identifier.</param>
/// <param name="EmployeeId">The employee identifier.</param>
/// <param name="Time">The logged time.</param>
/// <param name="Topic">The topic.</param>
/// <param name="Notes">Optional notes.</param>
/// <param name="SupervisorId">The supervisor identifier.</param>
/// <param name="TotalAcceptedTimeInMonth">The total accepted time for this contract in the current month.</param>
public sealed record TimeLogWaitingForAcceptationCreated(
    Ulid TimeLogId,
    Ulid ContractId,
    Ulid EmployeeId,
    TimeSpan Time,
    string Topic,
    string? Notes,
    Ulid SupervisorId,
    TimeSpan TotalAcceptedTimeInMonth) : IMessage;

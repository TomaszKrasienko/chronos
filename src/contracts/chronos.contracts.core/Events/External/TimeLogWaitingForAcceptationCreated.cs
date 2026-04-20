using chronos.contracts.core.DAL;
using chronos.shared.kernel.Identifiers;
using chronos.shared.messaging;
using Microsoft.Extensions.Logging;

namespace chronos.contracts.core.Events.External;

/// <summary>
/// Integration event raised when a time log waiting for acceptation is created.
/// </summary>
/// <param name="TimeLogId">The time log identifier.</param>
/// <param name="ContractId">The contract identifier.</param>
/// <param name="EmployeeId">The employee identifier.</param>
/// <param name="Hours">The logged hours.</param>
/// <param name="Topic">The topic.</param>
/// <param name="Notes">Optional notes.</param>
/// <param name="SupervisorId">The supervisor identifier.</param>
/// <param name="TotalAcceptedHoursInMonth">The total accepted hours for this contract in the current month.</param>
public sealed record TimeLogWaitingForAcceptationCreated(
    Ulid TimeLogId,
    Ulid ContractId,
    Ulid EmployeeId,
    TimeSpan Hours,
    string Topic,
    string? Notes,
    Ulid SupervisorId,
    TimeSpan TotalAcceptedHoursInMonth);

/// <summary>
/// Handler for <see cref="TimeLogWaitingForAcceptationCreated"/> integration event.
/// </summary>
public interface ITimeLogWaitingForAcceptationCreatedEventHandler
{
    Task HandleAsync(TimeLogWaitingForAcceptationCreated @event, CancellationToken cancellationToken = default);
}

internal sealed class TimeLogWaitingForAcceptationCreatedEventHandler(
    ILogger<TimeLogWaitingForAcceptationCreatedEventHandler> logger,
    IContractsRepository contractsRepository,
    IMessageDispatcher messageDispatcher) : ITimeLogWaitingForAcceptationCreatedEventHandler
{
    /// <inheritdoc />
    public async Task HandleAsync(TimeLogWaitingForAcceptationCreated @event, CancellationToken cancellationToken = default)
    {
        var contractId = new ContractId(@event.ContractId);
        var employeeId = new EmployeeId(@event.EmployeeId);
        var timeLogId = new TimeLogId(@event.TimeLogId);

        var contract = await contractsRepository.GetByIdAsync(contractId, cancellationToken);

        if (contract is null)
        {
            logger.LogWarning(
                "Contract with ID {ContractId} not found for time log {TimeLogId}. Rejecting automatically.",
                @event.ContractId,
                @event.TimeLogId);

            await messageDispatcher.Send(new TimeLogAutomaticallyRejected(timeLogId), cancellationToken);
            return;
        }

        var employee = contract.Employees.SingleOrDefault(e => e.Id == employeeId);

        if (employee is null)
        {
            logger.LogWarning(
                "Employee with ID {EmployeeId} not assigned to contract {ContractId}. Rejecting time log {TimeLogId} automatically.",
                @event.EmployeeId,
                @event.ContractId,
                @event.TimeLogId);

            await messageDispatcher.Send(new TimeLogAutomaticallyRejected(timeLogId), cancellationToken);
            return;
        }

        var allocatedHours = TimeSpan.FromHours(employee.AllocatedHours);
        var totalHoursAfterLog = @event.TotalAcceptedHoursInMonth + @event.Hours;

        if (totalHoursAfterLog > allocatedHours)
        {
            logger.LogInformation(
                "Time log {TimeLogId} exceeds allocated hours for employee {EmployeeId} on contract {ContractId}. " +
                "Allocated: {AllocatedHours}, Total after log: {TotalHoursAfterLog}. Rejecting automatically.",
                @event.TimeLogId,
                @event.EmployeeId,
                @event.ContractId,
                allocatedHours,
                totalHoursAfterLog);

            await messageDispatcher.Send(new TimeLogAutomaticallyRejected(timeLogId), cancellationToken);
            return;
        }

        logger.LogInformation(
            "Time log {TimeLogId} accepted automatically for employee {EmployeeId} on contract {ContractId}.",
            @event.TimeLogId,
            @event.EmployeeId,
            @event.ContractId);

        await messageDispatcher.Send(new TimeLogAutomaticallyAccepted(timeLogId), cancellationToken);
    }
}
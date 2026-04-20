using chronos.shared.kernel.Exceptions;
using chronos.shared.kernel.Identifiers;
using chronos.shared.messaging;
using chronos.time_logs.core.Communication.Sync;
using chronos.time_logs.core.DAL;
using chronos.time_logs.core.Domain;
using chronos.time_logs.core.Domain.Events;
using chronos.time_logs.core.Domain.ValueObjects;
using chronos.time_logs.core.Events.Mappers;

namespace chronos.time_logs.core.Services;

internal sealed class TimeReportService(
    ITimeLogsRepository timeLogsRepository,
    IContractsClient contractsClient,
    IEmployeesClient employeesClient,
    IMessageDispatcher messageDispatcher,
    TimeProvider timeProvider) : IReadTimeReportService, IWriteTimeReportService
{
    /// <inheritdoc />
    public async Task<TimeLogId> AddTimeLogAsync(
        EmployeeId employeeId,
        ContractId contractId,
        TimeSpan time,
        string topic,
        string? notes,
        CancellationToken cancellationToken = default)
    {
        if (!await contractsClient.IsEmployeeAssignedToContractAsync(
                contractId.Value, employeeId.Value, cancellationToken))
        {
            throw new NotFoundException(
                "EmployeeAssignment",
                [employeeId.Value.ToString(), contractId.Value.ToString()]);
        }

        var employee = await employeesClient.GetByIdAsync(employeeId.Value, cancellationToken);
        if (employee is null)
        {
            throw new NotFoundException("Employee", [employeeId.Value.ToString()]);
        }

        if (employee.SupervisorId is null)
        {
            throw new EmployeeHasNoSupervisorException([employeeId.Value.ToString()]);
        }

        var supervisorId = new EmployeeId(employee.SupervisorId.Value);

        var now = timeProvider.GetUtcNow();
        var report = await timeLogsRepository.GetByEmployeeAndPeriodAsync(
            employeeId, now.Month, now.Year, cancellationToken);

        var isNewReport = report is null;
        if (isNewReport)
        {
            var period = ReportPeriod.Create(now.Month, now.Year);
            report = MonthlyTimeReport.Create(employeeId, period);
        }

        var loggedTime = LoggedTime.Create(time);
        var timeLogId = report!.AddTimeLog(
            contractId,
            loggedTime,
            topic,
            notes,
            supervisorId,
            timeProvider);

        if (isNewReport)
        {
            await timeLogsRepository.AddAsync(report, cancellationToken);
        }
        else
        {
            await timeLogsRepository.UpdateAsync(report, cancellationToken);
        }

        var acceptedTime = report
            .TimeLogs
            .OfType<AcceptedTimeLog>()
            .Where(t => t.ContractId == contractId)
            .Sum(t => t.Time.Value.TotalHours);

        var domainEvent = report.DomainEvents
            .OfType<TimeLogWaitingForAcceptationCreatedEvent>()
            .Single();

        await messageDispatcher.Send(
            domainEvent.ToIntegrationEvent(TimeSpan.FromHours(acceptedTime)),
            cancellationToken);

        report.ClearDomainEvents();

        return timeLogId;
    }
    
    /// <inheritdoc />
    public async Task<MonthlyTimeReport?> GetByEmployeeAndPeriodAsync(
        EmployeeId employeeId,
        int month,
        int year,
        CancellationToken cancellationToken = default)
        => await timeLogsRepository.GetByEmployeeAndPeriodAsync(
            employeeId,
            month,
            year,
            cancellationToken);
}

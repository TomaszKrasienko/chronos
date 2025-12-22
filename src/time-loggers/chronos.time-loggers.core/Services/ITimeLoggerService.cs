using chronos.shared.messaging;
using chronos.time_loggers.core.Communication.Sync;
using chronos.time_loggers.core.DAL;
using chronos.time_loggers.core.Domain;
using chronos.time_loggers.core.Events;
using chronos.time_loggers.core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace chronos.time_loggers.core.Services;

public interface ITimeLoggerService
{
    Task<TimeLogger> CreateAsync(
        TimeSpan? timeSpan,
        TimeOnly? timeFrom,
        TimeOnly? timeTo,
        Ulid employeeId,
        string topic,
        string? notes,
        CancellationToken cancellationToken);
    
    Task AcceptAsync(
        Ulid id,
        Ulid supervisorId,
        CancellationToken cancellationToken);

    Task RejectAsync(
        Ulid id,
        Ulid supervisorId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TimeLogger>> GetByEmployeeIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<(Ulid EmployeeId, string FirstName, string LastName, IReadOnlyCollection<TimeLogger> TimeLogs)>> GetForSupervisorAsync(
        Ulid supervisorId,
        CancellationToken cancellationToken);
}

internal sealed class TimeLoggerService(
    TimeLoggersDbContext dbContext,
    IEmployeesClient employeesClient,
    IMessagePublisher messagePublisher) : ITimeLoggerService
{
    public async Task<TimeLogger> CreateAsync(
        TimeSpan? timeSpan,
        TimeOnly? timeFrom,
        TimeOnly? timeTo,
        Ulid employeeId,
        string topic,
        string? notes,
        CancellationToken cancellationToken)
    {
        var doesEmployeeExists = await employeesClient.AnyAsync(
            employeeId,
            cancellationToken);

        if (!doesEmployeeExists)
        {
            throw new InvalidOperationException("Employee not found");
        }
        
        var timeLogger = TimeLogger.Create(
            Ulid.NewUlid(),
            timeSpan,
            timeFrom,
            timeTo,
            employeeId,
            topic,
            notes);

        await dbContext.TimeLoggers.AddAsync(timeLogger, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var @event = new TimeLogCreated(
            timeLogger.Id,
            timeLogger.TimeSpan,
            timeLogger.EmployeeId,
            timeLogger.Topic,
            timeLogger.Notes);
        await messagePublisher.Send(@event, cancellationToken);
        
        return timeLogger;
    }

    public async Task AcceptAsync(
        Ulid id,
        Ulid supervisorId,
        CancellationToken cancellationToken)
    {
        var timeLogger = await dbContext.TimeLoggers
            .SingleOrDefaultAsync(
                t => t.Id == id,
                cancellationToken);

        if (timeLogger is null)
        {
            throw new TimeLoggerNotFoundException(id);
        }

        var employee = await employeesClient.GetByIdAsync(
            timeLogger.EmployeeId,
            cancellationToken);

        if (employee is null)
        {
            throw new InvalidOperationException($"Employee with ID {timeLogger.EmployeeId} not found");
        }

        if (employee.SupervisorId != supervisorId)
        {
            throw new UnauthorizedSupervisorException(supervisorId, employee.Id);
        }

        try
        {
            timeLogger.Accept();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidTimeLoggerStateException(timeLogger.Id, timeLogger.Status, "accept");
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var @event = new TimeLogAccepted(
            timeLogger.Id,
            timeLogger.TimeSpan,
            timeLogger.EmployeeId,
            timeLogger.Topic,
            timeLogger.Notes,
            supervisorId);
        await messagePublisher.Send(@event, cancellationToken);
    }

    public async Task RejectAsync(
        Ulid id,
        Ulid supervisorId,
        CancellationToken cancellationToken)
    {
        var timeLogger = await dbContext.TimeLoggers
            .SingleOrDefaultAsync(
                t => t.Id == id,
                cancellationToken);

        if (timeLogger is null)
        {
            throw new TimeLoggerNotFoundException(id);
        }

        var employee = await employeesClient.GetByIdAsync(
            timeLogger.EmployeeId,
            cancellationToken);

        if (employee is null)
        {
            throw new InvalidOperationException($"Employee with ID {timeLogger.EmployeeId} not found");
        }

        if (employee.SupervisorId != supervisorId)
        {
            throw new UnauthorizedSupervisorException(supervisorId, employee.Id);
        }

        try
        {
            timeLogger.Reject();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidTimeLoggerStateException(timeLogger.Id, timeLogger.Status, "reject");
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var @event = new TimeLogRejected(
            timeLogger.Id,
            timeLogger.TimeSpan,
            timeLogger.EmployeeId,
            timeLogger.Topic,
            timeLogger.Notes,
            supervisorId);
        await messagePublisher.Send(@event, cancellationToken);
    }

    public async Task<IReadOnlyCollection<TimeLogger>> GetByEmployeeIdAsync(
        Ulid employeeId,
        CancellationToken cancellationToken)
    {
        return await dbContext.TimeLoggers
            .AsNoTracking()
            .Where(tl => tl.EmployeeId == employeeId)
            .OrderByDescending(tl => tl.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<(Ulid EmployeeId, string FirstName, string LastName, IReadOnlyCollection<TimeLogger> TimeLogs)>> GetForSupervisorAsync(
        Ulid supervisorId,
        CancellationToken cancellationToken)
    {
        var subordinates = await employeesClient.GetSubordinatesAsync(
            supervisorId,
            cancellationToken);

        if (!subordinates.Any())
        {
            return [];
        }

        var orderedSubordinates = subordinates
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToList();

        var subordinateIds = orderedSubordinates.Select(e => e.Id).ToList();

        var timeLogs = await dbContext.TimeLoggers
            .AsNoTracking()
            .Where(tl => subordinateIds.Contains(tl.EmployeeId))
            .OrderByDescending(tl => tl.Id)
            .ToListAsync(cancellationToken);

        var groupedTimeLogs = orderedSubordinates.Select(employee =>
        {
            var employeeTimeLogs = timeLogs
                .Where(tl => tl.EmployeeId == employee.Id)
                .ToList();

            return (employee.Id, employee.FirstName, employee.LastName, (IReadOnlyCollection<TimeLogger>)employeeTimeLogs);
        }).ToList();

        return groupedTimeLogs;
    }
}

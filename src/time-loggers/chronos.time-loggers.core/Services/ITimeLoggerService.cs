using chronos.time_loggers.core.Communication.Sync;
using chronos.time_loggers.core.DAL;
using chronos.time_loggers.core.Domain;
using chronos.time_loggers.core.Events;

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
}

internal sealed class TimeLoggerService(
    TimeLoggersDbContext dbContext,
    IEmployeesClient employeesClient) : ITimeLoggerService
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
        
        return timeLogger;
    }
}

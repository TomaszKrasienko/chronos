using chronos.time_loggers.core.DAL;
using chronos.time_loggers.core.Domain;
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
}

internal sealed class TimeLoggerService(
    TimeLoggersDbContext dbContext) : ITimeLoggerService
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
        return timeLogger;
    }
}

using chronos.time_loggers.core.Domain;

namespace chronos.time_loggers.api;

public sealed record TimeLogDto(
    string Id,
    string TimeSpan,
    string EmployeeId,
    string Topic,
    string? Notes,
    string Status);

public static class TimeLoggerDtoExtensions
{
    public static TimeLogDto ToDto(this TimeLogger timeLogger)
    {
        return new TimeLogDto(
            timeLogger.Id.ToString(),
            timeLogger.TimeSpan.ToString(),
            timeLogger.EmployeeId.ToString(),
            timeLogger.Topic,
            timeLogger.Notes,
            timeLogger.Status.ToString());
    }
}

namespace chronos.time_loggers.api;

public sealed record EmployeeTimeLogsDto(
    string EmployeeId,
    string FirstName,
    string LastName,
    IReadOnlyCollection<TimeLogDto> TimeLogs);

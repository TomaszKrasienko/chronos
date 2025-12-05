namespace chronos.time_loggers.core.Events;

public sealed record TimeLogCreated(
    Ulid Id,
    TimeSpan TimeSpan,
    Ulid EmployeeId,
    string Topic,
    string? Notes);
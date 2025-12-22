namespace chronos.time_loggers.core.Events;

public sealed record TimeLogAccepted(
    Ulid Id,
    TimeSpan TimeSpan,
    Ulid EmployeeId,
    string Topic,
    string? Notes,
    Ulid AcceptedBySupervisorId);

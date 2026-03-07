namespace chronos.time_reports.core.Events;

public sealed record TimeLogRejected(
    Ulid Id,
    TimeSpan TimeSpan,
    Ulid EmployeeId,
    string Topic,
    string? Notes,
    Ulid RejectedBySupervisorId);

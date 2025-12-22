namespace chronos.notifications.core.Events;

public sealed record EmployeeCreated(
    Ulid Id,
    string Email,
    Ulid? SupervisorId);
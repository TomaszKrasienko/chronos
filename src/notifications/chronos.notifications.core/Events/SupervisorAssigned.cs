namespace chronos.notifications.core.Events;

public sealed record SupervisorAssigned(
    Ulid EmployeeId,
    Ulid SupervisorId);
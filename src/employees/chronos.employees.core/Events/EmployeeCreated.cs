namespace chronos.employees.core.Events;

public sealed record EmployeeCreated(
    Ulid Id,
    string FirstName,
    string LastName,
    string Email,
    Ulid? SupervisorId);
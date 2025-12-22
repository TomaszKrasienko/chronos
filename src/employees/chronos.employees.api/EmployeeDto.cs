namespace chronos.employees.api;

public sealed record EmployeeDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string? SupervisorId);
namespace chronos.employees.core.DTOs.Responses;

public sealed record EmployeeDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string? SupervisorId);

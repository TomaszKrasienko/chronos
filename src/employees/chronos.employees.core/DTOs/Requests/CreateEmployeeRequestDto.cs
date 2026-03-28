namespace chronos.employees.core.DTOs.Requests;

public sealed record CreateEmployeeRequestDto(
    string FirstName,
    string LastName,
    string Email,
    Ulid? SupervisorId);

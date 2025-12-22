public sealed record CreateEmployeeRequestDto(
    string FirstName,
    string LastName,
    string Email,
    Ulid? SupervisorId);
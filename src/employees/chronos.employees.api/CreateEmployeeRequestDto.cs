public sealed record CreateEmployeeRequestDto(
    string FirstName,
    string LastName,
    Ulid? SupervisorId);
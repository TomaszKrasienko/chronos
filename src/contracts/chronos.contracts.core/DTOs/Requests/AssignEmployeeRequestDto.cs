namespace chronos.contracts.core.DTOs.Requests;

public sealed record AssignEmployeeRequestDto(
    Ulid EmployeeId,
    DateOnly From,
    DateOnly? To,
    int AllocatedHours);

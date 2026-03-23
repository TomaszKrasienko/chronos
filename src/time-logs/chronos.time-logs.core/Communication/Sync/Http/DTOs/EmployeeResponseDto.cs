namespace chronos.time_logs.core.Communication.Sync.Http.DTOs;

public sealed record EmployeeResponseDto(
    string Id,
    string FirstName,
    string LastName,
    string? SupervisorId);
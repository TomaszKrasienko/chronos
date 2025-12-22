namespace chronos.time_loggers.core.Communication.Sync.Http.DTOs;

public sealed record EmployeeResponseDto(
    string Id,
    string FirstName,
    string LastName,
    string? SupervisorId);
namespace chronos.time_logs.core.DTOs.Responses;

public sealed record TimeLogResponseDto(
    string Id,
    string ContractId,
    TimeSpan Hours,
    string Topic,
    string? Notes,
    string Status,
    DateTimeOffset CreatedAt);

namespace chronos.time_logs.core.DTOs.Requests;

public sealed record CreateCurrentTimeLogRequestDto(
    Ulid ContractId,
    TimeSpan Hours,
    string Topic,
    string? Notes);

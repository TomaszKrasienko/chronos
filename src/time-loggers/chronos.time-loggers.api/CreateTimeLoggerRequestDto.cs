public sealed record CreateTimeLoggerRequestDto(
    TimeSpan? TimeSpan,
    TimeOnly? TimeFrom,
    TimeOnly? TimeTo,
    string Topic,
    string? Notes);

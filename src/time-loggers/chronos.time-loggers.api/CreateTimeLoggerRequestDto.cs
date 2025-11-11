public sealed record CreateTimeLoggerRequestDto(
    TimeSpan? TimeSpan,
    TimeOnly? TimeFrom,
    TimeOnly? TimeTo,
    Ulid EmployeeId,
    string Topic,
    string? Notes);

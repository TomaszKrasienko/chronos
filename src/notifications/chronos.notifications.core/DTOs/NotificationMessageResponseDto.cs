namespace chronos.notifications.core.DTOs;

public sealed record NotificationMessageResponseDto(
    Ulid Id,
    Ulid EmployeeId,
    string Topic,
    string Message,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ReadAt);
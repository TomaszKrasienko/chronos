namespace chronos.notifications.core.Domain;

public sealed class NotificationMessage
{
    public Ulid Id { get; }
    public Ulid EmployeeId { get; }
    public string Topic { get; }
    public string Message { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? ReadAt { get; private set; }

    private NotificationMessage(
        Ulid id,
        Ulid employeeId,
        string topic,
        string message,
        DateTimeOffset createdAt,
        DateTimeOffset? readAt)
    {
        Id = id;
        EmployeeId = employeeId;
        Topic = topic;
        Message = message;
        CreatedAt = createdAt;
        ReadAt = readAt;
    }

    public static NotificationMessage Create(
        Ulid id,
        Ulid employeeId,
        string topic,
        string message,
        TimeProvider timeProvider)
        => new NotificationMessage(
            id,
            employeeId,
            topic,
            message,
            timeProvider.GetUtcNow(),
            null);

    public void MarkAsRead(TimeProvider timeProvider)
        => ReadAt = timeProvider.GetUtcNow();
}
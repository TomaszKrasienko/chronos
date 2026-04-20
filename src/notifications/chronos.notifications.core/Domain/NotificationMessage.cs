using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.notifications.core.Domain;

/// <summary>
/// Entity representing a notification message within a contact.
/// </summary>
public sealed class NotificationMessage : Entity<NotificationMessageId>
{
    /// <summary>
    /// Gets the topic of the message.
    /// </summary>
    public string Topic { get; private set; }

    /// <summary>
    /// Gets the message content.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Gets the creation timestamp.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets the read timestamp, if the message has been read.
    /// </summary>
    public DateTimeOffset? ReadAt { get; private set; }

#pragma warning disable CS8618
    private NotificationMessage()
    {
    }
#pragma warning restore CS8618

    private NotificationMessage(
        NotificationMessageId id,
        string topic,
        string message,
        DateTimeOffset createdAt) : base(id)
    {
        Topic = topic;
        Message = message;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Creates a new notification message.
    /// </summary>
    /// <param name="topic">The topic of the message.</param>
    /// <param name="message">The message content.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>A new notification message.</returns>
    internal static NotificationMessage Create(
        string topic,
        string message,
        TimeProvider timeProvider)
        => new(
            NotificationMessageId.New(),
            topic,
            message,
            timeProvider.GetUtcNow());

    /// <summary>
    /// Marks the message as read.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    internal void MarkAsRead(TimeProvider timeProvider)
        => ReadAt = timeProvider.GetUtcNow();
}

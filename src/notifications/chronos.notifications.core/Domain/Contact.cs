using chronos.shared.kernel;
using chronos.shared.kernel.Identifiers;

namespace chronos.notifications.core.Domain;

/// <summary>
/// Aggregate root representing a contact for notifications.
/// </summary>
public sealed class Contact : AggregateRoot<ContactId>
{
    private readonly List<NotificationMessage> _messages = [];
    private readonly List<EmployeeId> _subordinates = [];

    /// <summary>
    /// Gets the email address of the contact.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Gets the notification messages for this contact.
    /// </summary>
    public IReadOnlyList<NotificationMessage> Messages => _messages.AsReadOnly();

    /// <summary>
    /// Gets the subordinates managed by this contact.
    /// </summary>
    public IReadOnlyList<EmployeeId> Subordinates => _subordinates.AsReadOnly();

#pragma warning disable CS8618
    private Contact()
    {
    }
#pragma warning restore CS8618

    private Contact(
        ContactId id,
        string email) : base(id)
    {
        Email = email;
    }

    /// <summary>
    /// Creates a new contact.
    /// </summary>
    /// <param name="id">The contact identifier (same as employee identifier).</param>
    /// <param name="email">The email address.</param>
    /// <returns>A new contact.</returns>
    public static Contact Create(
        ContactId id,
        string email)
        => new(id, email);

    /// <summary>
    /// Adds a subordinate to this contact.
    /// </summary>
    /// <param name="employeeId">The employee identifier to add as subordinate.</param>
    public void AddSubordinate(EmployeeId employeeId)
    {
        if (!_subordinates.Contains(employeeId))
        {
            _subordinates.Add(employeeId);
        }
    }

    /// <summary>
    /// Removes a subordinate from this contact.
    /// </summary>
    /// <param name="employeeId">The employee identifier to remove.</param>
    public void RemoveSubordinate(EmployeeId employeeId)
        => _subordinates.Remove(employeeId);

    /// <summary>
    /// Adds a notification message to this contact.
    /// </summary>
    /// <param name="topic">The topic of the message.</param>
    /// <param name="message">The message content.</param>
    /// <param name="timeProvider">The time provider.</param>
    public void AddMessage(
        string topic,
        string message,
        TimeProvider timeProvider)
    {
        var notificationMessage = NotificationMessage.Create(topic, message, timeProvider);
        _messages.Add(notificationMessage);
    }

    /// <summary>
    /// Marks a message as read.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="timeProvider">The time provider.</param>
    public void MarkMessageAsRead(
        NotificationMessageId messageId,
        TimeProvider timeProvider)
    {
        var message = _messages.SingleOrDefault(m => m.Id == messageId);
        message?.MarkAsRead(timeProvider);
    }
}

using System.Reflection;
using System.Text.Json;

namespace chronos.shared.messaging.outbox;

public sealed class OutboxMessage
{
    public Ulid MessageId { get; }
    public string JsonContent { get; }
    public string Type { get; }

    public object? Content => JsonSerializer.Deserialize(
        JsonContent,
        System.Type.GetType(Type)!) ?? null;
    
    public string Exchange { get; }
    public string? CorrelationId { get; }
    public string? RoutingKey { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? SentAt { get; private set; }
    public int RetryCount { get; private set; }
    public string? ErrorMessage { get; private set; }

    private OutboxMessage(
        Ulid messageId,
        string jsonContent,
        string type,
        string exchange,
        string? routingKey,
        string? correlationId,
        DateTimeOffset createdAt,
        DateTimeOffset? sentAt,
        int retryCount,
        string? errorMessage)
    {
        MessageId = messageId;
        JsonContent = jsonContent;
        Type = type;
        Exchange = exchange;
        CorrelationId = correlationId;
        RoutingKey = routingKey;
        CreatedAt = createdAt;
        SentAt = sentAt;
        RetryCount = retryCount;
        ErrorMessage = errorMessage;
    }

    public static OutboxMessage Create<T>(
        T content,
        string exchange,
        string? routingKey,
        string? correlationId,
        DateTimeOffset? sentAt,
        int retryCount,
        TimeProvider timeProvider) where T : class
    {
        var jsonContent = JsonSerializer.Serialize(content);
        var type = typeof(T).AssemblyQualifiedName!;
        var messageId = Ulid.NewUlid();
        var now = timeProvider.GetUtcNow();

        return new OutboxMessage(
            messageId,
            jsonContent,
            type,
            exchange,
            routingKey,
            correlationId,
            now,
            null,
            0,
            null);
    }

    public void MarkError(string errorMessage)
    {
        RetryCount++;
        ErrorMessage = errorMessage;
        SentAt = null;
    }

    public void MarkAsSent(TimeProvider timeProvider)
        => SentAt = timeProvider.GetUtcNow();
    
}
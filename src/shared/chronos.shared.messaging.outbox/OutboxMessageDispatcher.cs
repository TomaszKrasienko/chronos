using System.Text.RegularExpressions;
using chronos.shared.messaging;
using chronos.shared.messaging.outbox.DAL;

namespace chronos.shared.messaging.outbox;

/// <summary>
/// Implementation of <see cref="IMessageDispatcher"/> that stores messages in the outbox database
/// for reliable delivery.
/// </summary>
internal sealed class OutboxMessageDispatcher(
    OutboxDbContext dbContext,
    TimeProvider timeProvider,
    IMessagesRouteRegistry messagesRouteRegistry) : IMessageDispatcher
{
    /// <inheritdoc />
    public async Task Send<T>(
        T message,
        CancellationToken? cancellationToken = null) where T : class, IMessage
    {
        var messageRoute = messagesRouteRegistry
            .GetRoute<T>();

        if (messageRoute.RoutingKeys.Count == 0)
        {
            var outboxMessage = OutboxMessage.Create(
                message,
                messageRoute.Exchange,
                null,
                correlationId: null,
                sentAt: null,
                retryCount: 0,
                timeProvider);
            
            await dbContext.OutboxMessages.AddAsync(
                outboxMessage,
                cancellationToken ?? CancellationToken.None);
        }
        
        foreach (var routingKey in messageRoute.RoutingKeys)
        {
            var outboxMessage = OutboxMessage.Create(
                message,
                messageRoute.Exchange,
                routingKey,
                correlationId: null,
                sentAt: null,
                retryCount: 0,
                timeProvider);
            
            await dbContext.OutboxMessages.AddAsync(
                outboxMessage,
                cancellationToken ?? CancellationToken.None);
        }

        await dbContext.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
    }
}

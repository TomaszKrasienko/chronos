using chronos.shared.messaging.outbox.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace chronos.shared.messaging.outbox;

public sealed class OutboxProcessor(
    ILogger<OutboxProcessor> logger,
    IServiceProvider serviceProvider,
    TimeProvider timeProvider) : BackgroundService
{
    private const int BatchSize = 20;
    private const int MaxRetryCount = 5;
    private const int DelayMilliseconds = 5000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Outbox Processor started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<OutboxDbContext>();
                var messageProcessor = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                var messages = await dbContext
                    .OutboxMessages
                    .FromSql(
                        $"""
                        SELECT 
                            message_id
                          , json_content
                          , type
                          , exchange
                          , correlation_id
                          , routing_key
                          , created_at
                          , sent_at
                          , retry_count
                          , error_message
                        FROM outbox."outbox_messages"
                        WHERE sent_at IS NULL AND retry_count < {MaxRetryCount}
                        ORDER BY created_at
                        LIMIT {BatchSize}
                        FOR UPDATE SKIP LOCKED
                        """)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        var content = message.Content;

                        if (content is null)
                        {
                            message.MarkError("Invalid message type");
                            continue;
                        }

                        Task BasicReturn(object sender, BasicReturnEventArgs @event)
                        {
                            message.MarkError(@event.ReplyCode.ToString());
                            return Task.CompletedTask;
                        }

                        await messageProcessor.Send(
                            content,
                            message.Exchange,
                            message.RoutingKey ?? string.Empty,
                            BasicReturn,
                            stoppingToken);
                        
                        message.MarkAsSent(timeProvider);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex.Message, ex);
                        message.MarkError(ex.Message);
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing outbox messages.");
            }

            await Task.Delay(DelayMilliseconds, stoppingToken);
        }
    }
}
using RabbitMQ.Client;

namespace chronos.shared.messaging.rabbit_mq.AlternateExchange;

internal static class AlternateExchangeSetup
{
    private const string AlternateExchangeName = "chronos_alternate";

    internal static void AddAlternateExchangeArgument(Dictionary<string, object?> arguments)
    {
        arguments["alternate-exchange"] = AlternateExchangeName;
    }

    internal static async Task DeclareAlternateExchangeAsync(
        IChannel channel,
        CancellationToken cancellationToken = default)
    {
        await channel.ExchangeDeclareAsync(
            exchange: AlternateExchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);
    }
}

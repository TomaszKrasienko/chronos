namespace chronos.shared.messaging.rabbit_mq.Consumers;

public interface IMessagesRouteRegistry
{
    (string Exchange, string? Queue, List<string> RoutingKeys, bool IsTemporary) GetRoute<TMessage>() where TMessage : class;
}
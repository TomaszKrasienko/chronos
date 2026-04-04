namespace chronos.shared.messaging;

public interface IMessagesRouteRegistry
{
    (string Exchange, string? Queue, List<string> RoutingKeys, bool IsTemporary) GetRoute<TMessage>() where TMessage : class;
}
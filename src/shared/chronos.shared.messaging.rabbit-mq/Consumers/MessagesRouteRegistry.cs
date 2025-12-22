using chronos.shared.messaging.rabbit_mq.Configuration.Options;
using Microsoft.Extensions.Options;

namespace chronos.shared.messaging.rabbit_mq.Consumers;

internal sealed class MessagesRouteRegistry : IMessagesRouteRegistry
{
    private readonly Dictionary<Type, RouteInfo> _routes = new();

    public MessagesRouteRegistry(IOptions<RabbitMqOptions> options)
    {
        var rabbitMqOptions = options.Value;

        foreach (var (messageTypeName, routeOptions) in rabbitMqOptions.Routes)
        {
            var messageType = GetMessageType(messageTypeName);
            if (messageType is null)
            {
                continue;
            }

            var routingKeys = routeOptions.RoutingKeys.ToList();

            if (!routeOptions.IsTemporary &&
                string.IsNullOrWhiteSpace(routeOptions.Queue))
            {
                throw new ArgumentException("Not temporary queue must be defined");
            }
            
            _routes[messageType] = new RouteInfo(
                routeOptions.Exchange,
                routeOptions.Queue,
                routingKeys,
                routeOptions.IsTemporary);
        }
    }

    private static Type? GetMessageType(string messageTypeName)
    {
        var type = Type.GetType(messageTypeName);
        if (type is not null)
        {
            return type;
        }

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(messageTypeName);
            if (type is not null)
            {
                return type;
            }
        }

        return null;
    }

    public (string Exchange, string? Queue, List<string> RoutingKeys, bool IsTemporary) GetRoute<TMessage>() where TMessage : class
    {
        var messageType = typeof(TMessage);

        if (_routes.TryGetValue(messageType, out var route))
        {
            return (route.Exchange, route.Queue, route.RoutingKeys, route.IsTemporary);
        }

        return (string.Empty, null, [], false);
    }

    private sealed record RouteInfo(
        string Exchange,
        string? Queue,
        List<string> RoutingKeys,
        bool IsTemporary);
}

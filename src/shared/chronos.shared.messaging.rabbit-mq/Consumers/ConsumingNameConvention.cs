using System.Text.RegularExpressions;
using chronos.shared.configuration.Options;
using Microsoft.Extensions.Options;

namespace chronos.shared.messaging.rabbit_mq.Consumers;

internal sealed class ConsumingNameConvention : IConsumingNameConvention
{
    private readonly Guid _instanceId;
    private readonly IMessagesRouteRegistry _messagesRouteRegistry;

    public ConsumingNameConvention(
        IOptions<InstanceOptions> instanceOptions,
        IMessagesRouteRegistry messagesRouteRegistry)
    {
        _instanceId = instanceOptions.Value.Id;
        _messagesRouteRegistry = messagesRouteRegistry;
    }

    public string GetTemporaryQueueName<TMessage>() where TMessage : class
    {
        var messageType = typeof(TMessage);
        var typeName = messageType.Name;

        return $"{ToSnakeCase(typeName)}_temp_{_instanceId:N}";
    }

    public string GetQueueName<TMessage>() where TMessage : class
    {
        var queue = _messagesRouteRegistry
            .GetRoute<TMessage>()
            .Queue ?? throw new ArgumentException($"Cannot find non temporary queue for message type: {typeof(TMessage).Name}");

        return ToSnakeCase(queue);
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        var result = input.Replace(".", "_");
        result = Regex.Replace(result, "([a-z0-9])([A-Z])", "$1_$2");
        result = Regex.Replace(result, "([A-Z]+)([A-Z][a-z])", "$1_$2");

        return result.ToLowerInvariant();
    }
}

using System.Text.RegularExpressions;

namespace chronos.shared.messaging.rabbit_mq.Publishers;

public interface ISendingNameConvention
{
    (string exchange, string routingKey) GetExchangeAndRoutingKey<T>(T message) where T : class;   
}

internal sealed class SendingNameConvention : ISendingNameConvention
{
    public (string exchange, string routingKey) GetExchangeAndRoutingKey<T>(T message) where T : class
    {
        var messageType = typeof(T);
        var assemblyName = messageType.Assembly.GetName().Name ?? string.Empty;
        var typeName = messageType.Name;

        return (ToSnakeCase(assemblyName), ToSnakeCase(typeName));
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
        result = result.Replace("-", "_");

        return result.ToLowerInvariant();
    }
}

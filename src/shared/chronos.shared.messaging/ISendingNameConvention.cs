namespace chronos.shared.messaging;

public interface ISendingNameConvention
{
    (string exchange, string routingKey) GetExchangeAndRoutingKey<T>(T message) where T : class;   
}
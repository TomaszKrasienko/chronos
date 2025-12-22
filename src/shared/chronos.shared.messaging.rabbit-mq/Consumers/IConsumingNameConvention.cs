namespace chronos.shared.messaging.rabbit_mq.Consumers;

public interface IConsumingNameConvention
{
    string GetTemporaryQueueName<TMessage>() where TMessage : class;
    string GetQueueName<TMessage>() where TMessage : class;
}
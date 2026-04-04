namespace chronos.shared.messaging;

public interface IMessageDispatcher
{
    Task Send<T>(
        T message,
        CancellationToken? cancellationToken = null) where T : class, IMessage;
}
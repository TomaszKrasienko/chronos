namespace chronos.shared.messaging;

public interface IMessagePublisher
{
    Task Send<T>(
        T message,
        CancellationToken? cancellationToken = null) where T : class;
}
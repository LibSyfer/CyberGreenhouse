namespace CyberGreenhouse.MessageBus.Abstractions
{
    public interface IMonitorMessageBusHandler
    {
        Task Handle(IDictionary<string, object?> metadata, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken = default);
    }
}

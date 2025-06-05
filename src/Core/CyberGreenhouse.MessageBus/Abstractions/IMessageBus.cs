namespace CyberGreenhouse.MessageBus.Abstractions
{
    public interface IMessageBus
    {
        Task SendAsync(string destination, BusMessage message, CancellationToken cancellationToken = default);
    }
}

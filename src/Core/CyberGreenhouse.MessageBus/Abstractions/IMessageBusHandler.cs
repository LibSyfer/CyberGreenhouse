namespace CyberGreenhouse.MessageBus.Abstractions
{
    public interface IMessageBusHandler<in TBusMessage> : IMessageBusHandler
        where TBusMessage : BusMessage
    {
        Task IMessageBusHandler.Handle(BusMessage message, CancellationToken cancellationToken) => Handle((TBusMessage)message, cancellationToken);

        Task Handle(TBusMessage message, CancellationToken cancellationToken = default);
    }

    public interface IMessageBusHandler
    {
        Task Handle(BusMessage message, CancellationToken cancellationToken = default);
    }
}

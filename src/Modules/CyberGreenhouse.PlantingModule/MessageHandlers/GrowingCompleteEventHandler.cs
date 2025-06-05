using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events;

namespace CyberGreenhouse.PlantingModule.MessageHandlers
{
    public class GrowingCompleteEventHandler : IMessageBusHandler<GrowingCompleteEvent>
    {
        public Task Handle(GrowingCompleteEvent message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

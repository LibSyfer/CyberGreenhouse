using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Events;

namespace CyberGreenhouse.MainControl.MessageHandlers
{
    public class PlantReadyEventHandler : IMessageBusHandler<PlantReadyEvent>
    {
        private readonly ILogger<PlantReadyEventHandler> _logger;
        private readonly IMessageBus _messageBus;

        public PlantReadyEventHandler(ILogger<PlantReadyEventHandler> logger, IMessageBus messageBus)
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        public async Task Handle(PlantReadyEvent message, CancellationToken cancellationToken = default)
        {
            await _messageBus.SendAsync(ModuleNames.Harvesting, new StartHarvestingCommand(), cancellationToken);
        }
    }
}

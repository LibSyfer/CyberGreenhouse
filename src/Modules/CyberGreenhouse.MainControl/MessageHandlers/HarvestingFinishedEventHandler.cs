using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events;

namespace CyberGreenhouse.MainControl.MessageHandlers
{
    public class HarvestingFinishedEventHandler : IMessageBusHandler<HarvestingFinishedEvent>
    {
        private readonly ILogger<HarvestingFinishedEventHandler> _logger;
        private readonly GrowingService _growthingService;

        public HarvestingFinishedEventHandler(ILogger<HarvestingFinishedEventHandler> logger, GrowingService growthingService)
        {
            _logger = logger;
            _growthingService = growthingService;
        }

        public Task Handle(HarvestingFinishedEvent message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Growing finished");
            _growthingService.FinishGrow();

            return Task.CompletedTask;
        }
    }
}

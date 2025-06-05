using CyberGreenhouse.Core;
using CyberGreenhouse.GrowingCycleControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events;
using CyberGreenhouse.MessageBus.Contracts.Events.Harvesting;

namespace CyberGreenhouse.GrowingCycleControlModule.MessageHandlers
{
    public class HarvestingCompleteEventHandler : IMessageBusHandler<HarvestingCompleteEvent>
    {
        private readonly ILogger<HarvestingCompleteEventHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;

        public HarvestingCompleteEventHandler(ILogger<HarvestingCompleteEventHandler> logger, StateService stateService, IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
        }

        public async Task Handle(HarvestingCompleteEvent message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not GrowingCycleStatus.Harvesting)
            {
                _logger.LogWarning($"Cannot finish harvesting, state must be {GrowingCycleStatus.Harvesting.ToString()}, but in state: {_stateService.CurrentState.ToString()}");
                return;
            }

            _stateService.CurrentState = GrowingCycleStatus.Complete;
            _logger.LogInformation("Harvesting finished");
            await _messageBus.SendAsync(ModuleNames.MainControl, new GrowingCompleteEvent());
        }
    }
}

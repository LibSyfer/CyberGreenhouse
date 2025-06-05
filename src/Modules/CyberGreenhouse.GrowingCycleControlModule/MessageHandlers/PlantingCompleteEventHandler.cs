using CyberGreenhouse.Core;
using CyberGreenhouse.GrowingCycleControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.MaturityMonitoring;
using CyberGreenhouse.MessageBus.Contracts.Events.Planting;

namespace CyberGreenhouse.GrowingCycleControlModule.MessageHandlers
{
    public class PlantingCompleteEventHandler : IMessageBusHandler<PlantingCompleteEvent>
    {
        private readonly ILogger<PlantingCompleteEventHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;

        public PlantingCompleteEventHandler(ILogger<PlantingCompleteEventHandler> logger, StateService stateService, IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
        }

        public async Task Handle(PlantingCompleteEvent message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not GrowingCycleStatus.Planting)
            {
                _logger.LogWarning($"Cannot finish planting, state must be {GrowingCycleStatus.Planting.ToString()}, but in state: {_stateService.CurrentState.ToString()}");
                return;
            }

            if (_stateService.PlantGrowingParams is null)
            {
                _logger.LogError("Plant growing params is null");
                return;
            }

            _stateService.CurrentState = GrowingCycleStatus.Growing;
            _logger.LogInformation("Start maturity monitoring...");
            await _messageBus.SendAsync(ModuleNames.MaturityMonitoringControl, new StartMaturityMonitoringCommand
            {
                MinGrowthSeconds = _stateService.PlantGrowingParams.MinGrowthSeconds
            });
        }
    }
}

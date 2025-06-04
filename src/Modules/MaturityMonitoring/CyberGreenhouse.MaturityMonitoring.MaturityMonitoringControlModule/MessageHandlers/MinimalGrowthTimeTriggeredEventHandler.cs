using CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events.MaturityMonitoring;

namespace CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.MessageHandlers
{
    public class MinimalGrowthTimeTriggeredEventHandler : IMessageBusHandler<MinimalGrowthTimeTriggeredEvent>
    {
        private readonly ILogger<MinimalGrowthTimeTriggeredEventHandler> _logger;
        private readonly StateService _stateService;

        public MinimalGrowthTimeTriggeredEventHandler(ILogger<MinimalGrowthTimeTriggeredEventHandler> logger, StateService stateService)
        {
            _logger = logger;
            _stateService = stateService;
        }

        public Task Handle(MinimalGrowthTimeTriggeredEvent message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not MaturityMonitoringStatus.WaitTimeTrigger)
            {
                _logger.LogWarning($"Cannot set params, already in state: {_stateService.CurrentState.ToString()}");
                return Task.CompletedTask;
            }

            _logger.LogInformation("Change state to waiting visual trigger");
            _stateService.CurrentState = MaturityMonitoringStatus.WaitVisualTrigger;
            return Task.CompletedTask;
        }
    }
}

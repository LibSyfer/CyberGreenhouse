using CyberGreenhouse.Core;
using CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.MaturityMonitoring;

namespace CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.MessageHandlers
{
    public class StartMaturityMonitoringCommandHandler : IMessageBusHandler<StartMaturityMonitoringCommand>
    {
        private readonly ILogger<StartMaturityMonitoringCommandHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;

        public StartMaturityMonitoringCommandHandler(ILogger<StartMaturityMonitoringCommandHandler> logger, StateService stateService, IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
        }

        public async Task Handle(StartMaturityMonitoringCommand message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not MaturityMonitoringStatus.NotWork)
            {
                _logger.LogWarning($"Cannot set params, already in state: {_stateService.CurrentState.ToString()}");
                return;
            }

            _stateService.CurrentState = MaturityMonitoringStatus.WaitTimeTrigger;
            _logger.LogInformation("Send timer set command");
            await _messageBus.SendAsync(ModuleNames.TimeControl, new StartTimeControlCommand
            {
                MinGrowthSeconds = message.MinGrowthSeconds,
            });
        }
    }
}

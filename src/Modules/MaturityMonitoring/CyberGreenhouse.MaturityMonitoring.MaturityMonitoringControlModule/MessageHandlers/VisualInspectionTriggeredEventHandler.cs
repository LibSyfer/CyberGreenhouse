using CyberGreenhouse.Core;
using CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events.MaturityMonitoring;

namespace CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.MessageHandlers
{
    public class VisualInspectionTriggeredEventHandler : IMessageBusHandler<VisualInspectionTriggeredEvent>
    {
        private readonly ILogger<VisualInspectionTriggeredEventHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;

        public VisualInspectionTriggeredEventHandler(ILogger<VisualInspectionTriggeredEventHandler> logger, StateService stateService, IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
        }

        public async Task Handle(VisualInspectionTriggeredEvent message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not MaturityMonitoringStatus.WaitVisualTrigger)
            {
                _logger.LogWarning($"Cannot set params, state must be {MaturityMonitoringStatus.WaitVisualTrigger.ToString()}, but in state: {_stateService.CurrentState.ToString()}");
                return;
            }

            _logger.LogInformation("Maturity completed");
            await _messageBus.SendAsync(ModuleNames.MainControl, new MaturityCompletedEvent());

            _stateService.CurrentState = MaturityMonitoringStatus.NotWork;
        }
    }
}

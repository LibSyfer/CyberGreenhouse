using CyberGreenhouse.Core;
using CyberGreenhouse.GrowingCycleControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.Harvesting;
using CyberGreenhouse.MessageBus.Contracts.Events.MaturityMonitoring;

namespace CyberGreenhouse.GrowingCycleControlModule.MessageHandlers
{
    public class MaturityCompletedEventHandler : IMessageBusHandler<MaturityCompletedEvent>
    {
        private readonly ILogger<MaturityCompletedEventHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;

        public MaturityCompletedEventHandler(ILogger<MaturityCompletedEventHandler> logger, StateService stateService, IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
        }

        public async Task Handle(MaturityCompletedEvent message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not GrowingCycleStatus.Growing)
            {
                _logger.LogWarning($"Cannot finish maturity, state must be {GrowingCycleStatus.Growing.ToString()}, but in state: {_stateService.CurrentState.ToString()}");
                return;
            }

            _stateService.CurrentState = GrowingCycleStatus.Harvesting;
            _logger.LogInformation("Start harvesting...");
            await _messageBus.SendAsync(ModuleNames.HarvestingModule, new StartHarvestingCommand());
        }
    }
}

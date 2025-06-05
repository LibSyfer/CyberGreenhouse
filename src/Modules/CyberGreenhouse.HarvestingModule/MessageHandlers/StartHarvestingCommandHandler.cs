using CyberGreenhouse.Core;
using CyberGreenhouse.HarvestingModule.Models;
using CyberGreenhouse.HarvestingModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.Harvesting;
using CyberGreenhouse.MessageBus.Contracts.Events.Harvesting;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.HarvestingModule.MessageHandlers
{
    public class StartHarvestingCommandHandler : IMessageBusHandler<StartHarvestingCommand>
    {
        private readonly ILogger<StartHarvestingCommandHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;
        private readonly HarvestingSettings _harvestingSettings;

        public StartHarvestingCommandHandler(ILogger<StartHarvestingCommandHandler> logger,
            StateService stateService,
            IOptions<HarvestingSettings> harvestingSettingsOpt,
            IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
            _harvestingSettings = harvestingSettingsOpt.Value;
        }

        public async Task Handle(StartHarvestingCommand message, CancellationToken cancellationToken = default)
        {
            var currentState = _stateService.CurrentState;
            if (currentState is not HarvestingStatus.NotInitiated)
            {
                _logger.LogWarning($"Cannot initialize harvesting, state must be {HarvestingStatus.NotInitiated.ToString()}, but in state: {currentState.ToString()}");
                return;
            }

            if (_harvestingSettings.IsAutoComplete)
            {
                _stateService.CurrentState = HarvestingStatus.Complete;
                _logger.LogInformation("Enable auto complete mode, wait and complete");
                await Task.Delay(5000, cancellationToken);

                _logger.LogInformation("Harvesting complete");
                await _messageBus.SendAsync(ModuleNames.GrowingCycleControlModule, new HarvestingCompleteEvent());
                return;
            }

            _stateService.CurrentState = HarvestingStatus.Ready;
            _logger.LogInformation($"State set to {_stateService.CurrentState.ToString()}");
        }
    }
}

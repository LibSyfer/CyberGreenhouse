using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands.Planting;
using CyberGreenhouse.MessageBus.Contracts.Events.Planting;
using CyberGreenhouse.PlantingModule.Models;
using CyberGreenhouse.PlantingModule.Services;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.PlantingModule.MessageHandlers
{
    public class StartPlantingCommandHandler : IMessageBusHandler<StartPlantingCommand>
    {
        private readonly ILogger<StartPlantingCommandHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;
        private readonly PlantingSettings _plantingSettings;

        public StartPlantingCommandHandler(ILogger<StartPlantingCommandHandler> logger,
            StateService stateService,
            IOptions<PlantingSettings> plantingSettingsOpt,
            IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
            _plantingSettings = plantingSettingsOpt.Value;
        }

        public async Task Handle(StartPlantingCommand message, CancellationToken cancellationToken = default)
        {
            var currentState = _stateService.CurrentState;
            if (currentState is not PlantingStatus.NotInitiated)
            {
                _logger.LogWarning($"Cannot initialize planting, state must be {PlantingStatus.NotInitiated.ToString()}, but in state: {currentState.ToString()}");
                return;
            }

            if (_plantingSettings.IsAutoComplete)
            {
                _stateService.CurrentState = PlantingStatus.Complete;
                _logger.LogInformation("Enable auto complete mode, wait and complete");
                await Task.Delay(5000, cancellationToken);

                _logger.LogInformation("Planting complete");
                await _messageBus.SendAsync(ModuleNames.MainControl, new PlantingCompleteEvent());
                return;
            }

            _stateService.CurrentState = PlantingStatus.Ready;
            _logger.LogInformation($"State set to {_stateService.CurrentState.ToString()}");
        }
    }
}

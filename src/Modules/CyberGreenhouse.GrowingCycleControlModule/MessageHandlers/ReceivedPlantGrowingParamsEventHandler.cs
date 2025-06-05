using CyberGreenhouse.Core;
using CyberGreenhouse.GrowingCycleControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Commands.Planting;
using CyberGreenhouse.MessageBus.Contracts.Events;

namespace CyberGreenhouse.GrowingCycleControlModule.MessageHandlers
{
    public class ReceivedPlantGrowingParamsEventHandler : IMessageBusHandler<ReceivedPlantGrowingParamsEvent>
    {
        private readonly ILogger<ReceivedPlantGrowingParamsEventHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;

        public ReceivedPlantGrowingParamsEventHandler(ILogger<ReceivedPlantGrowingParamsEventHandler> logger, StateService stateService, IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
        }

        public async Task Handle(ReceivedPlantGrowingParamsEvent message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not GrowingCycleStatus.GettingParams)
            {
                _logger.LogWarning($"Cannot start received growing parameters, state must be {GrowingCycleStatus.GettingParams.ToString()}, but in state: {_stateService.CurrentState.ToString()}");
                return;
            }

            _stateService.CurrentState = GrowingCycleStatus.Planting;
            _logger.LogInformation("The growing parameters were getted");
            var growingParams = new PlantGrowingParams
            {
                Id = message.ParamId,
                TomatoId = message.TomatoId,
                LightIntensity = message.LightIntensity,
                LightDuration = message.LightDuration,
                AirTemperature = message.AirTemperature,
                WaterTemperature = message.WaterTemperature,
                HumidityLevel = message.HumidityLevel,
                SoilHumidity = message.SoilHumidity,
                FertilizerConcentrationPpm = message.FertilizerConcentrationPpm,
                MinGrowthSeconds = message.MinGrowthSeconds
            };

            _stateService.PlantGrowingParams = growingParams;

            _logger.LogInformation($"Send params for control systems to {ModuleNames.MainControl}");
            await _messageBus.SendAsync(ModuleNames.MainControl, new SetupAllControlModulesCommand
            {
                LightIntensity = message.LightIntensity,
                LightDuration = message.LightDuration,
                AirTemperature = message.AirTemperature,
                WaterTemperature = message.WaterTemperature,
                HumidityLevel = message.HumidityLevel,
                SoilHumidity = message.SoilHumidity,
                FertilizerConcentrationPpm = message.FertilizerConcentrationPpm
            });

            _logger.LogInformation($"Initiate planting");
            await _messageBus.SendAsync(ModuleNames.PlantingModule, new StartPlantingCommand());
        }
    }
}

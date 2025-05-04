using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Events;

namespace CyberGreenhouse.MainControl.MessageHandlers
{
    public class PlantingFinishedEventHandler : IMessageBusHandler<PlantingFinishedEvent>
    {
        private readonly ILogger<PlantingFinishedEventHandler> _logger;
        private readonly IMessageBus _messageBus;
        private readonly GrowingService _growthingService;

        public PlantingFinishedEventHandler(ILogger<PlantingFinishedEventHandler> logger, IMessageBus messageBus, GrowingService growthingService)
        {
            _logger = logger;
            _messageBus = messageBus;
            _growthingService = growthingService;
        }

        public async Task Handle(PlantingFinishedEvent message, CancellationToken cancellationToken = default)
        {
            var settingGrowingParams = _growthingService.GrowingParams;
            if (settingGrowingParams is null)
            {
                _logger.LogError("Null current params");
                _growthingService.FinishGrow();
                return;
            }

            await _messageBus.SendAsync(ModuleNames.Lighting, new SetLightingLevelCommand
            {
                LightIntensity = settingGrowingParams.LightIntensity,
            },
            cancellationToken);

            await _messageBus.SendAsync(ModuleNames.ClimateControl, new SetClimateControlParamsCommand
            {
                TemperatureDay = settingGrowingParams.TemperatureDay,
                TemperatureNight = settingGrowingParams.TemperatureNight,
                HumidityLevel = settingGrowingParams.HumidityLevel
            },
            cancellationToken);

            await _messageBus.SendAsync(ModuleNames.Irrigation, new SetIrrigationParamsCommand
            {
                WateringFrequency = settingGrowingParams.WateringFrequency,
                FertilizerType = settingGrowingParams.FertilizerType
            },
            cancellationToken);

            await _messageBus.SendAsync(ModuleNames.RipenessMonitor, new StartMonitorRipenessCommand(), cancellationToken);
        }
    }
}

using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Events;

namespace CyberGreenhouse.MainControl.MessageHandlers
{
    public class GettedPlantGrowingParamsEventHandler : IMessageBusHandler<GettedPlantGrowingParamsEvent>
    {
        private readonly ILogger<GettedPlantGrowingParamsEventHandler> _logger;
        private readonly IMessageBus _messageBus;
        private readonly GrowingService _growthingService;

        public GettedPlantGrowingParamsEventHandler(ILogger<GettedPlantGrowingParamsEventHandler> logger, IMessageBus messageBus, GrowingService growthingService)
        {
            _logger = logger;
            _messageBus = messageBus;
            _growthingService = growthingService;
        }

        public async Task Handle(GettedPlantGrowingParamsEvent message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("The growing parameters were getted");
            var growingParams = new PlantGrowingParams
            {
                Id = message.ParamId,
                TomatoId = message.TomatoId,
                LightIntensity = message.LightIntensity,
                LightDuration = message.LightDuration,
                TemperatureDay = message.TemperatureDay,
                TemperatureNight = message.TemperatureNight,
                HumidityLevel = message.HumidityLevel,
                WateringFrequency = message.WateringFrequency,
                FertilizerType = message.FertilizerType
            };

            _growthingService.GrowingParams = growingParams;

            _logger.LogInformation("Send start planting command");
            await _messageBus.SendAsync(ModuleNames.Planting, new StartPlantingCommand(), cancellationToken);
        }
    }
}

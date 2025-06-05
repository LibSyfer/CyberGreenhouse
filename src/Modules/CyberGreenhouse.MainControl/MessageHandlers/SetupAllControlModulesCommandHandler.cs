using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Commands.ClimateModule;
using CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation;
using CyberGreenhouse.MessageBus.Contracts.Commands.LightingModule;

namespace CyberGreenhouse.MainControl.MessageHandlers
{
    public class SetupAllControlModulesCommandHandler : IMessageBusHandler<SetupAllControlModulesCommand>
    {
        private readonly ILogger<SetupAllControlModulesCommandHandler> _logger;
        private readonly IMessageBus _messageBus;

        public SetupAllControlModulesCommandHandler(ILogger<SetupAllControlModulesCommandHandler> logger, IMessageBus messageBus)
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        public async Task Handle(SetupAllControlModulesCommand message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Setting lighting params...");
            await _messageBus.SendAsync(ModuleNames.LightingControl, new SetLightingLevelCommand
            {
                LightIntensity = message.LightIntensity,
                LightDuration = message.LightDuration,
            });

            _logger.LogInformation("Setting climate params...");
            await _messageBus.SendAsync(ModuleNames.ClimateControl, new SetClimateParamsCommand
            {
                AirTemperature = message.AirTemperature,
                WaterTemperature = message.WaterTemperature,
                HumidityLevel = message.HumidityLevel
            });

            _logger.LogInformation("Setting irrigation params...");
            await _messageBus.SendAsync(ModuleNames.IrrigationControl, new SetIrrigationParamsCommand
            {
                SoilHumidity = message.SoilHumidity,
                FertilizerConcentrationPpm = message.FertilizerConcentrationPpm
            });
        }
    }
}

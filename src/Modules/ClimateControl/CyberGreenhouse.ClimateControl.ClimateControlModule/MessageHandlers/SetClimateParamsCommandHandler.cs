using CyberGreenhouse.ClimateControl.ClimateControlModule.Models;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.ClimateModule;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.MessageHandlers
{
    public class SetClimateParamsCommandHandler : IMessageBusHandler<SetClimateParamsCommand>
    {
        private readonly ILogger<SetClimateParamsCommandHandler> _logger;
        private readonly ClimateSettings _climateSettings;
        private readonly RequiredClimateSettings _requiredClimateSettings;

        public SetClimateParamsCommandHandler(ILogger<SetClimateParamsCommandHandler> logger, IOptions<ClimateSettings> climateSettingsOpt, RequiredClimateSettings requiredClimateSettings)
        {
            _logger = logger;
            _climateSettings = climateSettingsOpt.Value;
            _requiredClimateSettings = requiredClimateSettings;
        }

        public Task Handle(SetClimateParamsCommand message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Set required climate settings. Air temperature: {@AirTemperature}. Water temperature: {@WaterTemperature}. Humidity level: {@HumidityLevel}", message.AirTemperature, message.WaterTemperature, message.HumidityLevel);
            if (message.AirTemperature > _climateSettings.MaxAirTemperature || message.AirTemperature < _climateSettings.MinAirTemperature)
            {
                _logger.LogWarning("Air temperature out of range: {@AirTemperature}, required range: {@MinAirTemperature} - {@MaxAirTemperature}.", message.AirTemperature, _climateSettings.MinAirTemperature, _climateSettings.MaxAirTemperature);
            }
            else
            {
                _requiredClimateSettings.RequiredAirTemperature = message.AirTemperature;
            }

            if (message.WaterTemperature > _climateSettings.MaxWaterTemperature || message.WaterTemperature < _climateSettings.MinWaterTemperature)
            {
                _logger.LogWarning("Water temperature out of range: {@WaterTemperature}, required range: {@MinWaterTemperature} - {@MaxWaterTemperature}.", message.WaterTemperature, _climateSettings.MinWaterTemperature, _climateSettings.MaxWaterTemperature);
            }
            else
            {
                _requiredClimateSettings.RequiredWaterTemperature = message.WaterTemperature;
            }

            if (message.HumidityLevel > _climateSettings.MaxHumidityLevel || message.HumidityLevel < _climateSettings.MinHumidityLevel)
            {
                _logger.LogWarning("Humidity level out of range: {@HumidityLevel}, required range: {@MinHumidityLevel} - {@MaxHumidityLevel}.", message.HumidityLevel, _climateSettings.MinHumidityLevel, _climateSettings.MaxHumidityLevel);
            }
            else
            {
                _requiredClimateSettings.RequiredHumidityLevel = message.HumidityLevel;
            }

            return Task.CompletedTask;
        }
    }
}

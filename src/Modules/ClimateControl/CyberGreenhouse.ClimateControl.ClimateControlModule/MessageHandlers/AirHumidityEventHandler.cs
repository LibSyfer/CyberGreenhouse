using CyberGreenhouse.ClimateControl.ClimateControlModule.Models;
using CyberGreenhouse.ClimateControl.ClimateControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;
using CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.MessageHandlers
{
    public class AirHumidityEventHandler : IMessageBusHandler<AirHumidityEvent>
    {
        private readonly ILogger<AirHumidityEventHandler> _logger;
        private readonly ClimateSettings _climateSettings;
        private readonly HumiditingAirControllerService _humiditingAirControllerService;
        private readonly RequiredClimateSettings _requiredClimateSettings;
        private readonly IMessageBus _messageBus;

        public AirHumidityEventHandler(ILogger<AirHumidityEventHandler> logger, IOptions<ClimateSettings> climateSettingsOpt, HumiditingAirControllerService humiditingAirControllerService, RequiredClimateSettings requiredClimateSettings, IMessageBus messageBus)
        {
            _logger = logger;
            _climateSettings = climateSettingsOpt.Value;
            _humiditingAirControllerService = humiditingAirControllerService;
            _requiredClimateSettings = requiredClimateSettings;
            _messageBus = messageBus;
        }

        public async Task Handle(AirHumidityEvent message, CancellationToken cancellationToken = default)
        {

            if (message.Humidity < (_requiredClimateSettings.RequiredHumidityLevel - _climateSettings.DeviationHumidityLevel)
                || message.Humidity > (_requiredClimateSettings.RequiredHumidityLevel + _climateSettings.DeviationHumidityLevel))
            {
                if (_requiredClimateSettings.CurrentHumidityStabilizationAttempt == _requiredClimateSettings.StabilizationAttempts)
                {
                    await _messageBus.SendAsync(ModuleNames.EmergencyStop, new AbordSystemCommand
                    {
                        ModuleName = ModuleNames.ClimateControl,
                        ErrorMessage = "Cannot stabilize humidity level"
                    });

                    return;
                }

                if (message.Humidity < (_requiredClimateSettings.RequiredHumidityLevel - _climateSettings.DeviationHumidityLevel))
                {
                    if (message.Humidity < _climateSettings.MinHumidityLevel)
                    {
                        _logger.LogError("Dangerous humidity level");
                    }

                    _requiredClimateSettings.CurrentHumidityStabilizationAttempt++;
                    _humiditingAirControllerService.Increase();
                }
                else if (message.Humidity > (_requiredClimateSettings.RequiredHumidityLevel + _climateSettings.DeviationHumidityLevel))
                {
                    if (message.Humidity > _climateSettings.MaxHumidityLevel)
                    {
                        _logger.LogError("Dangerous humidity level");
                    }

                    _requiredClimateSettings.CurrentHumidityStabilizationAttempt++;
                    _humiditingAirControllerService.Decrease();
                }
            }
        }
    }
}

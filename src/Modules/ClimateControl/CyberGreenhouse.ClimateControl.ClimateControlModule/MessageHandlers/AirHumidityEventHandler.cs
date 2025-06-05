using CyberGreenhouse.ClimateControl.ClimateControlModule.Controllers;
using CyberGreenhouse.ClimateControl.ClimateControlModule.Models;
using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
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
            if (message.Humidity < _climateSettings.MinHumidityLevel || message.Humidity > _climateSettings.MaxHumidityLevel)
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
                _logger.LogError($"Dangerous humidity level ({message.Humidity}). Try stabilize. Attempt: {_requiredClimateSettings.CurrentHumidityStabilizationAttempt + 1}/{_requiredClimateSettings.StabilizationAttempts}");
                _requiredClimateSettings.CurrentHumidityStabilizationAttempt++;
            }

            if (message.Humidity < (_requiredClimateSettings.RequiredHumidityLevel - _climateSettings.DeviationHumidityLevel))
            {
                _humiditingAirControllerService.Increase();
            }
            else if (message.Humidity > (_requiredClimateSettings.RequiredHumidityLevel + _climateSettings.DeviationHumidityLevel))
            {
                _humiditingAirControllerService.Decrease();
            }
            else
            {
                _requiredClimateSettings.CurrentHumidityStabilizationAttempt = 0;
            }
        }
    }
}

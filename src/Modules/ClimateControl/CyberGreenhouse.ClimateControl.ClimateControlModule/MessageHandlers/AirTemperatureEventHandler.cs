using CyberGreenhouse.ClimateControl.ClimateControlModule.Controllers;
using CyberGreenhouse.ClimateControl.ClimateControlModule.Models;
using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;
using CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.MessageHandlers
{
    public class AirTemperatureEventHandler : IMessageBusHandler<AirTemperatureEvent>
    {
        private readonly ILogger<AirTemperatureEventHandler> _logger;
        private readonly ClimateSettings _climateSettings;
        private readonly HeatingAirControllerService _heatingAirControllerService;
        private readonly FreezingAirControllerService _freezingAirControllerService;
        private readonly RequiredClimateSettings _requiredClimateSettings;
        private readonly IMessageBus _messageBus;

        public AirTemperatureEventHandler(ILogger<AirTemperatureEventHandler> logger, IOptions<ClimateSettings> climateSettingsOpt, HeatingAirControllerService heatingAirControllerService, FreezingAirControllerService freezingAirControllerService, RequiredClimateSettings requiredClimateSettings, IMessageBus messageBus)
        {
            _logger = logger;
            _climateSettings = climateSettingsOpt.Value;
            _heatingAirControllerService = heatingAirControllerService;
            _freezingAirControllerService = freezingAirControllerService;
            _requiredClimateSettings = requiredClimateSettings;
            _messageBus = messageBus;
        }

        public async Task Handle(AirTemperatureEvent message, CancellationToken cancellationToken = default)
        {
            if (message.Temperature < _climateSettings.MinAirTemperature || message.Temperature > _climateSettings.MaxAirTemperature)
            {
                if (_requiredClimateSettings.CurrentAirStabilizationAttempt == _requiredClimateSettings.StabilizationAttempts)
                {
                    await _messageBus.SendAsync(ModuleNames.EmergencyStop, new AbordSystemCommand
                    {
                        ModuleName = ModuleNames.ClimateControl,
                        ErrorMessage = "Cannot stabilize air temperature"
                    });

                    return;
                }
                _logger.LogError($"Dangerous air temperature ({message.Temperature}). Try stabilize. Attempt: {_requiredClimateSettings.CurrentAirStabilizationAttempt + 1}/{_requiredClimateSettings.StabilizationAttempts}");
                _requiredClimateSettings.CurrentAirStabilizationAttempt++;
            }

            if (message.Temperature < (_requiredClimateSettings.RequiredAirTemperature - _climateSettings.DeviationAirTemperature))
            {
                _heatingAirControllerService.Heat();
            }
            else if (message.Temperature > (_requiredClimateSettings.RequiredAirTemperature + _climateSettings.DeviationAirTemperature))
            {
                _freezingAirControllerService.Freez();
            }
            else
            {
                _requiredClimateSettings.CurrentAirStabilizationAttempt = 0;
            }
        }
    }
}

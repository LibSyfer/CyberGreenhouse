using CyberGreenhouse.ClimateControl.ClimateControlModule.Models;
using CyberGreenhouse.ClimateControl.ClimateControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
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

            if (message.Temperature < (_requiredClimateSettings.RequiredAirTemperature - _climateSettings.DeviationAirTemperature)
                || message.Temperature > (_requiredClimateSettings.RequiredAirTemperature + _climateSettings.DeviationAirTemperature))
            {
                if (_requiredClimateSettings.CurrentStabilizationAttempt == _requiredClimateSettings.StabilizationAttempts)
                {
                    await _messageBus.SendAsync(ModuleNames.EmergencyStop, new AbordSystemCommand
                    {
                        ModuleName = ModuleNames.ClimateControl,
                        ErrorMessage = "Cannot stabilize air temperature"
                    });

                    return;
                }

                if (message.Temperature < (_requiredClimateSettings.RequiredAirTemperature - _climateSettings.DeviationAirTemperature))
                {
                    if (message.Temperature < _climateSettings.MinAirTemperature)
                    {
                        _logger.LogError("Dangerous air temperature");
                    }

                    _requiredClimateSettings.CurrentStabilizationAttempt++;
                    _heatingAirControllerService.Heat();
                }
                else if (message.Temperature > (_requiredClimateSettings.RequiredAirTemperature + _climateSettings.DeviationAirTemperature))
                {
                    if (message.Temperature > _climateSettings.MinAirTemperature)
                    {
                        _logger.LogError("Dangerous air temperature");
                    }

                    _requiredClimateSettings.CurrentStabilizationAttempt++;
                    _freezingAirControllerService.Freez();
                }
            }
        }
    }
}

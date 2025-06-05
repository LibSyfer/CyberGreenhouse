using CyberGreenhouse.ClimateControl.ClimateControlModule.Controllers;
using CyberGreenhouse.ClimateControl.ClimateControlModule.Models;
using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;
using CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.MessageHandlers
{
    public class WaterTemperatureEventHandler : IMessageBusHandler<WaterTemperatureEvent>
    {
        private readonly ILogger<WaterTemperatureEventHandler> _logger;
        private readonly ClimateSettings _climateSettings;
        private readonly HeatingWaterControllerService _heatingWaterControllerService;
        private readonly FreezingWaterControllerService _freezingWaterControllerService;
        private readonly RequiredClimateSettings _requiredClimateSettings;
        private readonly IMessageBus _messageBus;

        public WaterTemperatureEventHandler(ILogger<WaterTemperatureEventHandler> logger, IOptions<ClimateSettings> climateSettingsOpt, HeatingWaterControllerService heatingWaterControllerService, FreezingWaterControllerService freezingWaterControllerService, RequiredClimateSettings requiredClimateSettings, IMessageBus messageBus)
        {
            _logger = logger;
            _climateSettings = climateSettingsOpt.Value;
            _heatingWaterControllerService = heatingWaterControllerService;
            _freezingWaterControllerService = freezingWaterControllerService;
            _requiredClimateSettings = requiredClimateSettings;
            _messageBus = messageBus;
        }

        public async Task Handle(WaterTemperatureEvent message, CancellationToken cancellationToken = default)
        {

            if (message.Temperature < (_requiredClimateSettings.RequiredWaterTemperature - _climateSettings.DeviationWaterTemperature)
                || message.Temperature > (_requiredClimateSettings.RequiredWaterTemperature + _climateSettings.DeviationWaterTemperature))
            {
                if (_requiredClimateSettings.CurrentWaterStabilizationAttempt == _requiredClimateSettings.StabilizationAttempts)
                {
                    await _messageBus.SendAsync(ModuleNames.EmergencyStop, new AbordSystemCommand
                    {
                        ModuleName = ModuleNames.ClimateControl,
                        ErrorMessage = "Cannot stabilize water temperature"
                    });

                    return;
                }

                if (message.Temperature < (_requiredClimateSettings.RequiredWaterTemperature - _climateSettings.DeviationWaterTemperature))
                {
                    if (message.Temperature < _climateSettings.MinWaterTemperature)
                    {
                        _logger.LogError("Dangerous water temperature");
                    }

                    _requiredClimateSettings.CurrentWaterStabilizationAttempt++;
                    _heatingWaterControllerService.Heat();
                }
                else if (message.Temperature > (_requiredClimateSettings.RequiredWaterTemperature + _climateSettings.DeviationWaterTemperature))
                {
                    if (message.Temperature > _climateSettings.MaxWaterTemperature)
                    {
                        _logger.LogError("Dangerous water temperature");
                    }

                    _requiredClimateSettings.CurrentWaterStabilizationAttempt++;
                    _freezingWaterControllerService.Freez();
                }
            }
        }
    }
}

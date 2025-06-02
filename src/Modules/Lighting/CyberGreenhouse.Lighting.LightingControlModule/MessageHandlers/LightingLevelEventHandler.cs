using CyberGreenhouse.Lighting.LightingControlModule.Models;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events.LightingModule;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.Lighting.LightingControlModule.MessageHandlers
{
    public class LightingLevelEventHandler : IMessageBusHandler<LightingLevelEvent>
    {
        private readonly ILogger<LightingLevelEventHandler> _logger;
        private readonly LightingSettings _lightingSettings;
        private readonly LightingControllerService _lightingControllerService;
        private readonly RequiredLightingSettings _requiredLightingSettings;

        public LightingLevelEventHandler(ILogger<LightingLevelEventHandler> logger, IOptions<LightingSettings> lightingSettingsOpt, LightingControllerService lightingControllerService, RequiredLightingSettings requiredLightingSettings)
        {
            _logger = logger;
            _lightingSettings = lightingSettingsOpt.Value;
            _lightingControllerService = lightingControllerService;
            _requiredLightingSettings = requiredLightingSettings;
        }

        public Task Handle(LightingLevelEvent message, CancellationToken cancellationToken = default)
        {
            if (message.LightIntensity > _lightingSettings.MaxLightIntensity || message.LightIntensity < _lightingSettings.MinLightIntensity)
            {
                _logger.LogWarning("Light intensity out of range: {@CurrentLightIntensity}, required range: {@MinLightIntensity} - {@MaxLightIntensity}.", message.LightIntensity, _lightingSettings.MinLightIntensity, _lightingSettings.MaxLightIntensity);

                var requiredLightIntensity = _requiredLightingSettings.RequiredLightIntensity;
                _logger.LogInformation("Set required Light intensity: {@RequiredLightIntensity}", requiredLightIntensity);
                _lightingControllerService.SetLightIntensity(requiredLightIntensity);
            }

            return Task.CompletedTask;
        }
    }
}

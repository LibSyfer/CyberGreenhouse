using CyberGreenhouse.Lighting.LightingControlModule.Models;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.LightingModule;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.Lighting.LightingControlModule.MessageHandlers
{
    public class SetLightingLevelCommandHandler : IMessageBusHandler<SetLightingLevelCommand>
    {
        private readonly ILogger<SetLightingLevelCommandHandler> _logger;
        private readonly LightingSettings _lightingSettings;
        private readonly RequiredLightingSettings _requiredLightingSettings;

        public SetLightingLevelCommandHandler(ILogger<SetLightingLevelCommandHandler> logger, IOptions<LightingSettings> lightingOptions, RequiredLightingSettings requiredLightingSettings)
        {
            _logger = logger;
            _lightingSettings = lightingOptions.Value;
            _requiredLightingSettings = requiredLightingSettings;
        }

        public Task Handle(SetLightingLevelCommand message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Set required lighting settings. Light intensity: {@LightIntensity}. Light duration: {@LightDuration}", message.LightIntensity, message.LightDuration);
            if (message.LightIntensity > _lightingSettings.MaxLightIntensity || message.LightIntensity < _lightingSettings.MinLightIntensity)
            {
                _logger.LogWarning("Light intensity out of range: {@LightIntensity}, required range: {@MinLightIntensity} - {@MaxLightIntensity}.", message.LightIntensity, _lightingSettings.MinLightIntensity, _lightingSettings.MaxLightIntensity);

                return Task.CompletedTask;
            }

            _requiredLightingSettings.RequiredLightIntensity = message.LightIntensity;
            return Task.CompletedTask;
        }
    }
}

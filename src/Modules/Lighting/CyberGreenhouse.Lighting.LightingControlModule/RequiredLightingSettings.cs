using CyberGreenhouse.Lighting.LightingControlModule.Models;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.Lighting.LightingControlModule
{
    public class RequiredLightingSettings
    {
        public RequiredLightingSettings(IOptions<LightingSettings> lightingSettingsOpt)
        {
            RequiredLightIntensity = lightingSettingsOpt.Value.MinLightIntensity + (lightingSettingsOpt.Value.MaxLightIntensity - lightingSettingsOpt.Value.MinLightIntensity) / 2;
        }

        public double RequiredLightIntensity { get; set; }
    }
}

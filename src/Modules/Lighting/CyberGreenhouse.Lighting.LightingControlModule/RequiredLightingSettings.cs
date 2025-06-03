using CyberGreenhouse.Lighting.LightingControlModule.Models;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.Lighting.LightingControlModule
{
    public class RequiredLightingSettings
    {
        public RequiredLightingSettings(IOptions<LightingSettings> lightingSettingsOpt)
        {
            RequiredLightIntensity = AvarageValue(lightingSettingsOpt.Value.MinLightIntensity, lightingSettingsOpt.Value.MaxLightIntensity);
        }

        public double RequiredLightIntensity { get; set; }

        private double AvarageValue(double min, double max)
        {
            return min + (max - min) / 2;
        }
    }
}

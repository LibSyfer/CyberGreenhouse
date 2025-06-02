namespace CyberGreenhouse.Lighting.LightingControlModule.Models
{
    public class LightingSettings
    {
        public const string Section = "LightingSettings";

        public double MinLightIntensity { get; set; }

        public double MaxLightIntensity { get; set; }

        public int MinLightDuration { get; set; }

        public int MaxLightDuration { get; set; }
    }
}

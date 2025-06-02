using Microsoft.Extensions.Options;

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Models
{
    public class RequiredClimateSettings
    {
        public RequiredClimateSettings(IOptions<ClimateSettings> climateSettingsOpt, int stabilizationAttempts = 5)
        {
            var climateSettings = climateSettingsOpt.Value;
            RequiredAirTemperature = AvarageValue(climateSettings.MinAirTemperature, climateSettings.MaxAirTemperature);
            RequiredWaterTemperature = AvarageValue(climateSettings.MinWaterTemperature, climateSettings.MaxWaterTemperature);
            RequiredHumidityLevel = AvarageValue(climateSettings.MinHumidityLevel, climateSettings.MaxHumidityLevel);

            CurrentStabilizationAttempt = 0;
            CurrentWaterStabilizationAttempt = 0;
            CurrentAirStabilizationAttempt = 0;
            CurrentHumidityStabilizationAttempt = 0;
            StabilizationAttempts = stabilizationAttempts;
        }

        public double RequiredAirTemperature { get; set; }

        public double RequiredWaterTemperature { get; set; }

        public double RequiredHumidityLevel { get; set; }

        public int CurrentStabilizationAttempt { get; set; }

        public int CurrentWaterStabilizationAttempt { get; set; }

        public int CurrentAirStabilizationAttempt { get; set; }

        public int CurrentHumidityStabilizationAttempt { get; set; }

        public int StabilizationAttempts { get; set; }

        private double AvarageValue(double min, double max)
        {
            return min + (max - min) / 2;
        }
    }
}

using Microsoft.Extensions.Options;

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Models
{
    public class RequiredClimateSettings
    {
        private readonly ILogger<RequiredClimateSettings> _logger;

        public RequiredClimateSettings(ILogger<RequiredClimateSettings> logger, IOptions<ClimateSettings> climateSettingsOpt, int stabilizationAttempts = 7)
        {
            _logger = logger;

            var climateSettings = climateSettingsOpt.Value;
            RequiredAirTemperature = AvarageValue(climateSettings.MinAirTemperature, climateSettings.MaxAirTemperature);
            RequiredWaterTemperature = AvarageValue(climateSettings.MinWaterTemperature, climateSettings.MaxWaterTemperature);
            RequiredHumidityLevel = AvarageValue(climateSettings.MinHumidityLevel, climateSettings.MaxHumidityLevel);

            _logger.LogInformation($"Set required params AirTemperature: {RequiredAirTemperature}, WaterTemperature: {RequiredWaterTemperature}, HumidityLevel: {RequiredHumidityLevel}");

            CurrentWaterStabilizationAttempt = 0;
            CurrentAirStabilizationAttempt = 0;
            CurrentHumidityStabilizationAttempt = 0;
            StabilizationAttempts = stabilizationAttempts;
        }

        private readonly object _lockObj = new object();
        private int _currentWaterStabilizationAttempt;
        private int _currentAirStabilizationAttempt;
        private int _currentHumidityStabilizationAttempt;

        public double RequiredAirTemperature { get; set; }

        public double RequiredWaterTemperature { get; set; }

        public double RequiredHumidityLevel { get; set; }

        public int CurrentWaterStabilizationAttempt
        {
            get { lock (_lockObj) return _currentWaterStabilizationAttempt; }
            set { lock (_lockObj) _currentWaterStabilizationAttempt = value; }
        }

        public int CurrentAirStabilizationAttempt
        {
            get { lock (_lockObj) return _currentAirStabilizationAttempt; }
            set { lock (_lockObj) _currentAirStabilizationAttempt = value; }
        }

        public int CurrentHumidityStabilizationAttempt
        {
            get { lock (_lockObj) return _currentHumidityStabilizationAttempt; }
            set { lock (_lockObj) _currentHumidityStabilizationAttempt = value; }
        }

        public int StabilizationAttempts { get; set; }

        private double AvarageValue(double min, double max)
        {
            return min + (max - min) / 2;
        }
    }
}

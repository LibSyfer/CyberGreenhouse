using Microsoft.Extensions.Options;

namespace CyberGreenhouse.Irrigation.IrrigationControlModule.Models
{
    public class RequiredIrrigationSettings
    {
        private readonly ILogger<RequiredIrrigationSettings> _logger;

        public RequiredIrrigationSettings(ILogger<RequiredIrrigationSettings> logger, IOptions<IrrigationSettings> irrigationSettingsOpt, int stabilizationAttempts = 7)
        {
            _logger = logger;

            var irrigationSettings = irrigationSettingsOpt.Value;
            RequiredSoilHumidity = AvarageValue(irrigationSettings.MinSoilHumidity, irrigationSettings.MaxSoilHumidity);
            RequiredFertilizerConcentrationPpm = AvarageValue(irrigationSettings.MinFertilizerConcentrationPpm, irrigationSettings.MaxFertilizerConcentrationPpm);

            _logger.LogInformation($"Set required params SoilHumidity: {RequiredSoilHumidity}, FertilizerConcentrationPpm: {RequiredFertilizerConcentrationPpm}");

            CurrentSoilHumidityStabilizationAttempt = 0;
            StabilizationAttempts = stabilizationAttempts;
        }

        public double RequiredSoilHumidity { get; set; }

        public double RequiredFertilizerConcentrationPpm { get; set; }

        public int CurrentSoilHumidityStabilizationAttempt { get; set; }

        public int StabilizationAttempts { get; set; }

        private double AvarageValue(double min, double max)
        {
            return min + (max - min) / 2;
        }
    }
}

using Microsoft.Extensions.Options;

namespace CyberGreenhouse.Irrigation.IrrigationControlModule.Models
{
    public class RequiredIrrigationSettings
    {
        public RequiredIrrigationSettings(IOptions<IrrigationSettings> irrigationSettingsOpt, int stabilizationAttempts = 5)
        {
            var irrigationSettings = irrigationSettingsOpt.Value;
            RequiredSoilHumidity = AvarageValue(irrigationSettings.MinSoilHumidity, irrigationSettings.MaxSoilHumidity);
            RequiredFertilizerConcentrationPpm = AvarageValue(irrigationSettings.MinFertilizerConcentrationPpm, irrigationSettings.MaxFertilizerConcentrationPpm);

            CurrentSoilHumidityStabilizationAttempt = 0;
            CurrentFertilizerConcentratioStabilizationAttempt = 0;
            StabilizationAttempts = stabilizationAttempts;
        }

        public double RequiredSoilHumidity { get; set; }

        public double RequiredFertilizerConcentrationPpm { get; set; }

        public int CurrentSoilHumidityStabilizationAttempt { get; set; }

        public int CurrentFertilizerConcentratioStabilizationAttempt { get; set; }

        public int StabilizationAttempts { get; set; }

        private double AvarageValue(double min, double max)
        {
            return min + (max - min) / 2;
        }
    }
}

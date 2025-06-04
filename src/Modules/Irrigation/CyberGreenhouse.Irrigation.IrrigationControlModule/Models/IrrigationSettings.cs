namespace CyberGreenhouse.Irrigation.IrrigationControlModule.Models
{
    public class IrrigationSettings
    {
        public const string Section = "IrrigationSettings";

        public double MinSoilHumidity { get; set; }

        public double MaxSoilHumidity { get; set; }

        public double DeviationSoilHumidity { get; set; }

        public double MinFertilizerConcentrationPpm { get; set; }

        public double MaxFertilizerConcentrationPpm { get; set; }

        public double DeviationFertilizerConcentrationPpm { get; set; }
    }
}

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Models
{
    public class ClimateSettings
    {
        public const string Section = "ClimateSettings";

        public double MinAirTemperature { get; set; }

        public double MaxAirTemperature { get; set; }

        public double DeviationAirTemperature { get; set; }

        public double MinWaterTemperature { get; set; }

        public double MaxWaterTemperature { get; set; }

        public double DeviationWaterTemperature { get; set; }

        public double MinHumidityLevel { get; set; }

        public double MaxHumidityLevel { get; set; }

        public double DeviationHumidityLevel { get; set; }
    }
}

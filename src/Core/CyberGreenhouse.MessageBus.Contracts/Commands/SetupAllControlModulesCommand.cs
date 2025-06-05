using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands
{
    public class SetupAllControlModulesCommand : BusMessage
    {
        public double LightIntensity { get; set; }                  // Интенсивность света в люксах

        public int LightDuration { get; set; }                      //Продолжительность светового дня в часах

        public double AirTemperature { get; set; }

        public double WaterTemperature { get; set; }

        public double HumidityLevel { get; set; }

        public double SoilHumidity { get; set; }

        public double FertilizerConcentrationPpm { get; set; }
    }
}

using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands
{
    public class SetClimateControlParamsCommand : BusMessage
    {
        public double TemperatureDay { get; set; }

        public double TemperatureNight { get; set; }

        public double HumidityLevel { get; set; }
    }
}

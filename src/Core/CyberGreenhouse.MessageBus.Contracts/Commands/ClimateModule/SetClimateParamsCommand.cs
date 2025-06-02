using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands.ClimateModule
{
    public class SetClimateParamsCommand : BusMessage
    {
        public double AirTemperature { get; set; }

        public double WaterTemperature { get; set; }

        public double HumidityLevel { get; set; }
    }
}

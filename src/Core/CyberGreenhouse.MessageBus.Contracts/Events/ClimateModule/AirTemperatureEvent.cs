using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule
{
    public class AirTemperatureEvent : BusMessage
    {
        public double Temperature { get; set; }
    }
}

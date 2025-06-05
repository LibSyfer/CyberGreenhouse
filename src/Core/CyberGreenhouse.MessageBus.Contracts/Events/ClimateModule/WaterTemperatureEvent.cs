using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule
{
    public class WaterTemperatureEvent : BusMessage
    {
        public double Temperature { get; set; }
    }
}

using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Events.Irrigation
{
    public class SoilHumidityEvent : BusMessage
    {
        public double CurrentSoilHumidity { get; set; }
    }
}

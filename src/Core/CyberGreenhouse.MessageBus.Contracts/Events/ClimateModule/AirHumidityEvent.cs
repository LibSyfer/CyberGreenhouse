using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule
{
    public class AirHumidityEvent : BusMessage
    {
        public double Humidity { get; set; }
    }
}

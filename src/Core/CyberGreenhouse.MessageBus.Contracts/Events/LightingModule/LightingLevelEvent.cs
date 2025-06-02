using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Events.LightingModule
{
    public class LightingLevelEvent : BusMessage
    {
        public double LightIntensity { get; set; }
    }
}

using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Events
{
    public class LightingLevelTelemetryEvent : BusMessage
    {
        public double CurrentLightIntensity { get; set; }

        public int RemainingLightDuration { get; set; }
    }
}

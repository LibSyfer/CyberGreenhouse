namespace CyberGreenhouse.MessageBus.Contracts.Events
{
    public class ClimateControlParamsTelemetryEvent
    {
        public double CurrentTemperature { get; set; }

        public double CurrentHumidityLevel { get; set; }
    }
}

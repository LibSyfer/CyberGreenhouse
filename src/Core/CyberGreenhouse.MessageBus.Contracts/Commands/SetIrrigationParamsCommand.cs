using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands
{
    public class SetIrrigationParamsCommand : BusMessage
    {
        public double WateringFrequency { get; set; }

        public string FertilizerType { get; set; } = string.Empty;
    }
}

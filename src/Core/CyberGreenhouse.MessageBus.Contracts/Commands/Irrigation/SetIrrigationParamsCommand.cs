using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation
{
    public class SetIrrigationParamsCommand : BusMessage
    {
        public double WateringFrequency { get; set; }

        public string FertilizerType { get; set; } = string.Empty;

        public double FertilizerConcentrationPpm { get; set; }
    }
}

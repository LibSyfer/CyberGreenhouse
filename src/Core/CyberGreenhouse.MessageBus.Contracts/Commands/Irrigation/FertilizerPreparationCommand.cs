using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation
{
    public class FertilizerPreparationCommand : BusMessage
    {
        public string FertilizerType { get; set; } = string.Empty;

        public double FertilizerConcentrationPpm { get; set; }
    }
}

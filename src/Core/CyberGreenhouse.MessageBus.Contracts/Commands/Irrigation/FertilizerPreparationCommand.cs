using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation
{
    public class FertilizerPreparationCommand : BusMessage
    {
        public double FertilizerConcentrationPpm { get; set; }
    }
}

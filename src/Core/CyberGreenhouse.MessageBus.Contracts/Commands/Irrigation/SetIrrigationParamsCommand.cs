using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation
{
    public class SetIrrigationParamsCommand : BusMessage
    {
        public double SoilHumidity { get; set; }

        public double FertilizerConcentrationPpm { get; set; }
    }
}

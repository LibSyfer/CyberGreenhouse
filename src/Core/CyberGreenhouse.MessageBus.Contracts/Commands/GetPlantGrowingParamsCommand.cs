using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands
{
    public class GetPlantGrowingParamsCommand: BusMessage
    {
        public Guid ParamId { get; set; }
    }
}

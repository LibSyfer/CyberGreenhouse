using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands
{
    public class StartGrowingCycleCommand : BusMessage
    {
        public Guid ParamId { get; set; }
    }
}

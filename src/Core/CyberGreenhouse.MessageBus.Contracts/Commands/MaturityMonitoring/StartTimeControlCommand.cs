using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands.MaturityMonitoring
{
    public class StartTimeControlCommand : BusMessage
    {
        public int MinGrowthSeconds { get; set; }
    }
}

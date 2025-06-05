using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands.MaturityMonitoring
{
    public class StartMaturityMonitoringCommand : BusMessage
    {
        public int MinGrowthSeconds { get; set; }
    }
}

using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands
{
    public class AbordCommand : BusMessage
    {
        public string Message { get; set; }
    }
}

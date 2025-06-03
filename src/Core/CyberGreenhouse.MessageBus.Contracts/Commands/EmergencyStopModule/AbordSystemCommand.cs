using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule
{
    public class AbordSystemCommand : BusMessage
    {
        public string ModuleName { get; set; }

        public string ErrorMessage { get; set; }
    }
}

using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;

namespace CyberGreenhouse.EmergencyStopModule.MessageHandlers
{
    public class AbordSystemCommandHandler : IMessageBusHandler<AbordSystemCommand>
    {
        private readonly ILogger<AbordSystemCommandHandler> _logger;

        public AbordSystemCommandHandler(ILogger<AbordSystemCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(AbordSystemCommand message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Abord: from {message.ModuleName}, message: {message.ErrorMessage}");
            return Task.CompletedTask;
        }
    }
}

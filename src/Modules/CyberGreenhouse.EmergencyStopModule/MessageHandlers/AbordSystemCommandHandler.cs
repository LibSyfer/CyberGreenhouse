using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;

namespace CyberGreenhouse.EmergencyStopModule.MessageHandlers
{
    public class AbordSystemCommandHandler : IMessageBusHandler<AbordSystemCommand>
    {
        private readonly ILogger<AbordSystemCommandHandler> _logger;
        private readonly IMessageBus _messageBus;

        public AbordSystemCommandHandler(ILogger<AbordSystemCommandHandler> logger, IMessageBus messageBus  )
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        public async Task Handle(AbordSystemCommand message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Abord: from {message.ModuleName}, message: {message.ErrorMessage}");
            await _messageBus.SendAsync(ModuleNames.MainControl, new AbordSystemCommand
            {
                ModuleName = message.ModuleName,
                ErrorMessage = message.ErrorMessage
            });
        }
    }
}

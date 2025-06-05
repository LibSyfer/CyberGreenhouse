using CyberGreenhouse.MainControl.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;

namespace CyberGreenhouse.MainControl.MessageHandlers
{
    public class AbordSystemCommandHandler : IMessageBusHandler<AbordSystemCommand>
    {
        private readonly ILogger<AbordSystemCommandHandler> _logger;
        private readonly StateService _stateService;

        public AbordSystemCommandHandler(ILogger<AbordSystemCommandHandler> logger, StateService stateService)
        {
            _logger = logger;
            _stateService = stateService;
        }

        public Task Handle(AbordSystemCommand message, CancellationToken cancellationToken = default)
        {
            _logger.LogError($"Abord system in module: {message.ModuleName} {message.ErrorMessage}");

            _stateService.CurrentState = MainControlStatus.Aborded;
            _stateService.ErrorModule = message.ModuleName;
            _stateService.Error = message.ErrorMessage;
            return Task.CompletedTask;
        }
    }
}

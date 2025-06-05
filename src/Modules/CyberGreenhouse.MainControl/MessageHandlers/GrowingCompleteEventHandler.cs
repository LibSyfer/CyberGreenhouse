using CyberGreenhouse.MainControl.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events;

namespace CyberGreenhouse.MainControl.MessageHandlers
{
    public class GrowingCompleteEventHandler : IMessageBusHandler<GrowingCompleteEvent>
    {
        private readonly ILogger<GrowingCompleteEventHandler> _logger;
        private readonly StateService _stateService;

        public GrowingCompleteEventHandler(ILogger<GrowingCompleteEventHandler> logger, StateService stateService)
        {
            _logger = logger;
            _stateService = stateService;
        }

        public Task Handle(GrowingCompleteEvent message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Growing completed");
            _stateService.CurrentState = MainControlStatus.Completed;
            return Task.CompletedTask;
        }
    }
}

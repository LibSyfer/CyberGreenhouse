using CyberGreenhouse.Irrigation.IrrigationControlModule.Service;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events.Irrigation;

namespace CyberGreenhouse.Irrigation.IrrigationControlModule.MessageHandlers
{
    public class FertilizerPreparationCompleteEventHandler : IMessageBusHandler<FertilizerPreparationCompleteEvent>
    {
        private readonly ILogger<FertilizerPreparationCompleteEventHandler> _logger;
        private readonly StateService _stateService;

        public FertilizerPreparationCompleteEventHandler(ILogger<FertilizerPreparationCompleteEventHandler> logger,
            StateService stateService,
            IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
        }

        public Task Handle(FertilizerPreparationCompleteEvent message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fertilizer preparation complete");
            _stateService.CurrentState = IrrigationStatus.Work;
            return Task.CompletedTask;
        }
    }
}

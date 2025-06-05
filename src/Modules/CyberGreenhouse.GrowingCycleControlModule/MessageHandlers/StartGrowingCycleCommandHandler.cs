using CyberGreenhouse.Core;
using CyberGreenhouse.GrowingCycleControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands;

namespace CyberGreenhouse.GrowingCycleControlModule.MessageHandlers
{
    public class StartGrowingCycleCommandHandler : IMessageBusHandler<StartGrowingCycleCommand>
    {
        private readonly ILogger<StartGrowingCycleCommandHandler> _logger;
        private readonly StateService _stateService;
        private readonly IMessageBus _messageBus;

        public StartGrowingCycleCommandHandler(ILogger<StartGrowingCycleCommandHandler> logger, StateService stateService, IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _messageBus = messageBus;
        }

        public async Task Handle(StartGrowingCycleCommand message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not GrowingCycleStatus.NotWork)
            {
                _logger.LogWarning($"Cannot start growing cycle, growing already start, state must be {GrowingCycleStatus.NotWork.ToString()}, but in state: {_stateService.CurrentState.ToString()}");
                return;
            }

            _stateService.CurrentState = GrowingCycleStatus.GettingParams;
            _logger.LogInformation("Getting params");
            await _messageBus.SendAsync(ModuleNames.PlantDataSignatureChecker, new GetPlantGrowingParamsCommand
            {
                ParamId = message.ParamId,
            });
        }
    }
}

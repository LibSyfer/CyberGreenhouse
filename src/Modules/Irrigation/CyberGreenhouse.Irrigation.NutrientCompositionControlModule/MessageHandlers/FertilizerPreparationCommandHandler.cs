using CyberGreenhouse.Core;
using CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers;
using CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation;
using CyberGreenhouse.MessageBus.Contracts.Events.Irrigation;

namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.MessageHandlers
{
    public class FertilizerPreparationCommandHandler : IMessageBusHandler<FertilizerPreparationCommand>
    {
        private readonly ILogger<FertilizerPreparationCommandHandler> _logger;
        private readonly FertilizerSupplyControllerService _fertilizerSupplyController;
        private readonly MixerControllerService _mixerControllerService;
        private readonly TankCleanerControllerService _tankCleanerControllerService;
        private readonly WaterSupplyControllerService _waterSupplyControllerService;
        private readonly StateService _tankParams;
        private readonly IMessageBus _messageBus;

        public FertilizerPreparationCommandHandler(ILogger<FertilizerPreparationCommandHandler> logger,
            FertilizerSupplyControllerService fertilizerSupplyControllerService,
            MixerControllerService mixerControllerService,
            TankCleanerControllerService tankCleanerControllerService,
            WaterSupplyControllerService waterSupplyControllerService,
            StateService tankParams,
            IMessageBus messageBus)
        {
            _logger = logger;
            _fertilizerSupplyController = fertilizerSupplyControllerService;
            _mixerControllerService = mixerControllerService;
            _tankCleanerControllerService = tankCleanerControllerService;
            _waterSupplyControllerService = waterSupplyControllerService;
            _tankParams = tankParams;
            _messageBus = messageBus;
        }

        public async Task Handle(FertilizerPreparationCommand message, CancellationToken cancellationToken = default)
        {
            if (_tankParams.CurrentState is not TankState.NotWork)
                return;

            _tankParams.CurrentState = TankState.Cleaning;
            _logger.LogInformation("Start cleaning tank");
            await _tankCleanerControllerService.CleanTank(cancellationToken);
            _logger.LogInformation("Finish cleaning tank");

            _tankParams.CurrentState = TankState.SupplyWater;
            _logger.LogInformation("Start supplying water");
            await _waterSupplyControllerService.Supply();
            _logger.LogInformation("Finish supplying water");


            _tankParams.CurrentState = TankState.SupplyFertilizer;
            _logger.LogInformation("Start suppling fertilizer");
            await _fertilizerSupplyController.Supply(cancellationToken);
            _logger.LogInformation("Finish supplying fertilizer");

            _tankParams.CurrentState = TankState.Mixing;
            _logger.LogInformation("Start mixing");
            await _mixerControllerService.Mix(cancellationToken);
            _logger.LogInformation("Finish mixing");

            _tankParams.CurrentState = TankState.NotWork;
            await _messageBus.SendAsync(ModuleNames.IrrigationControl, new FertilizerPreparationCompleteEvent());
        }
    }
}

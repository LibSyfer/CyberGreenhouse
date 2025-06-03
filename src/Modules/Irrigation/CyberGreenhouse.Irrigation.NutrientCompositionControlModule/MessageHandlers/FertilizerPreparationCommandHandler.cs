using CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation;

namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.MessageHandlers
{
    public class FertilizerPreparationCommandHandler : IMessageBusHandler<FertilizerPreparationCommand>
    {
        private readonly ILogger<FertilizerPreparationCommandHandler> _logger;
        private readonly FertilizerSupplyController _fertilizerSupplyController;
        private readonly MixerControllerService _mixerControllerService;
        private readonly IMessageBus _messageBus;

        public Task Handle(FertilizerPreparationCommand message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

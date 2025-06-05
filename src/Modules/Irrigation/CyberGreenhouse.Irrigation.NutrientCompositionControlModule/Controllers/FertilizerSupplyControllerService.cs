using System.Threading;

namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class FertilizerSupplyControllerService
    {
        private readonly ILogger<FertilizerSupplyControllerService> _logger;

        public FertilizerSupplyControllerService(ILogger<FertilizerSupplyControllerService> logger)
        {
            _logger = logger;
        }

        public async Task Supply(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Supplying fertilizer...");

            await Task.Delay(10000, cancellationToken);

            _logger.LogInformation("Supplying fertilizer finished");
        }
    }
}

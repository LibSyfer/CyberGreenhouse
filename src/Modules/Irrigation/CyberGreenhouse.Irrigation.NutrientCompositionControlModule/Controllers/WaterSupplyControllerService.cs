using System.Threading;

namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class WaterSupplyControllerService
    {
        private readonly ILogger<WaterSupplyControllerService> _logger;

        public WaterSupplyControllerService(ILogger<WaterSupplyControllerService> logger)
        {
            _logger = logger;
        }

        public async Task Supply(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Supplying water...");

            await Task.Delay(10000, cancellationToken);

            _logger.LogInformation("Supplying fertilizer finished");
        }
    }
}

namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class WaterSupplyControllerService
    {
        private readonly ILogger<WaterSupplyControllerService> _logger;

        public WaterSupplyControllerService(ILogger<WaterSupplyControllerService> logger)
        {
            _logger = logger;
        }

        public void StartSupply()
        {
            _logger.LogInformation("Start suppying water");
        }

        public void FinishSupply()
        {
            _logger.LogInformation("Finish suppying water");
        }
    }
}

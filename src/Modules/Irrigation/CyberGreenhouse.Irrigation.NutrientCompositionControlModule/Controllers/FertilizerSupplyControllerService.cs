namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class FertilizerSupplyControllerService
    {
        private readonly ILogger<FertilizerSupplyControllerService> _logger;

        public FertilizerSupplyControllerService(ILogger<FertilizerSupplyControllerService> logger)
        {
            _logger = logger;
        }

        public void Supply()
        {
            _logger.LogInformation("Fertilizer supply");
        }
    }
}

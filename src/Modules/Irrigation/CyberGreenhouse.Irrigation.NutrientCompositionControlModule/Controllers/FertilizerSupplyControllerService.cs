namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class FertilizerSupplyControllerService
    {
        private readonly ILogger<FertilizerSupplyControllerService> _logger;

        public FertilizerSupplyControllerService(ILogger<FertilizerSupplyControllerService> logger)
        {
            _logger = logger;
        }

        public void Supply(int fertilizerSection, double weightKg)
        {
            _logger.LogInformation($"Fertilizer supply kg: {weightKg} from section {fertilizerSection}");
        }
    }
}

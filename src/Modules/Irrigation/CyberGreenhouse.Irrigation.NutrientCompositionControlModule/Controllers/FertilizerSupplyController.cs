namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class FertilizerSupplyController
    {
        private readonly ILogger<FertilizerSupplyController> _logger;

        public FertilizerSupplyController(ILogger<FertilizerSupplyController> logger)
        {
            _logger = logger;
        }

        public void Supply(int fertilizerSection, double weightKg)
        {
            _logger.LogInformation($"Fertilizer supply kg: {weightKg} from section {fertilizerSection}");
        }
    }
}

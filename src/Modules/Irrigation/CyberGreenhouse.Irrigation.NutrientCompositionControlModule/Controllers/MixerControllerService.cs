namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class MixerControllerService
    {
        private readonly ILogger<MixerControllerService> _logger;

        public MixerControllerService(ILogger<MixerControllerService> logger)
        {
            _logger = logger;
        }

        public void Mix()
        {
            _logger.LogInformation("Start mixing fertilizer");
        }
    }
}

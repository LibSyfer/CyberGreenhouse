namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class MixerControllerService
    {
        private readonly ILogger<MixerControllerService> _logger;

        public MixerControllerService(ILogger<MixerControllerService> logger)
        {
            _logger = logger;
        }

        public async Task Mix(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Mixing fertilizer...");

            await Task.Delay(10000, cancellationToken);

            _logger.LogInformation("Mixing fertilizer finished");
        }
    }
}

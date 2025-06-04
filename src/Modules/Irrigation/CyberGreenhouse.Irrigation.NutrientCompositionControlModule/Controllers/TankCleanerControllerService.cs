namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers
{
    public class TankCleanerControllerService
    {
        private readonly ILogger<TankCleanerControllerService> _logger;

        public TankCleanerControllerService(ILogger<TankCleanerControllerService> logger)
        {
            _logger = logger;
        }

        public async Task CleanTank(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Cleaning tank...");

            await Task.Delay(10000, cancellationToken);

            _logger.LogInformation("Cleaning finished");
        }
    }
}

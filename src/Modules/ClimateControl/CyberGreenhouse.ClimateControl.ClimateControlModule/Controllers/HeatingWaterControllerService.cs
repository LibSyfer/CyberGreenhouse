namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Controllers
{
    public class HeatingWaterControllerService
    {
        private readonly ILogger<HeatingWaterControllerService> _logger;

        public HeatingWaterControllerService(ILogger<HeatingWaterControllerService> logger)
        {
            _logger = logger;
        }

        public void Heat()
        {
            _logger.LogInformation("Heating water");
        }
    }
}

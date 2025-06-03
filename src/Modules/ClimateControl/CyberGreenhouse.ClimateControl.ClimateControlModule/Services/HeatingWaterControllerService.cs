namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Services
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

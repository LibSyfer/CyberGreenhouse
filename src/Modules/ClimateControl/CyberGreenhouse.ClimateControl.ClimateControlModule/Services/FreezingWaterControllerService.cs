namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Services
{
    public class FreezingWaterControllerService
    {
        private readonly ILogger<FreezingAirControllerService> _logger;

        public FreezingWaterControllerService(ILogger<FreezingAirControllerService> logger)
        {
            _logger = logger;
        }

        public void Freez()
        {
            _logger.LogInformation("Freezing water");
        }
    }
}

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Controllers
{
    public class FreezingAirControllerService
    {
        private readonly ILogger<FreezingAirControllerService> _logger;

        public FreezingAirControllerService(ILogger<FreezingAirControllerService> logger)
        {
            _logger = logger;
        }

        public void Freez()
        {
            _logger.LogInformation("Freezing air");
        }
    }
}

namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Controllers
{
    public class HeatingAirControllerService
    {
        private readonly ILogger<HeatingAirControllerService> _logger;

        public HeatingAirControllerService(ILogger<HeatingAirControllerService> logger)
        {
            _logger = logger;
        }

        public void Heat()
        {
            _logger.LogInformation("Heating air");
        }
    }
}

namespace CyberGreenhouse.Lighting.LightingControlModule.Controllers
{
    public class LightingControllerService
    {
        private readonly ILogger<LightingControllerService> _logger;

        public LightingControllerService(ILogger<LightingControllerService> logger)
        {
            _logger = logger;
        }

        public void SetLightIntensity(double intensity)
        {
            _logger.LogInformation("Set light intensity: {@Intensity}", intensity);
        }

        public void TurnOnLight()
        {
            _logger.LogInformation("Turn on light");
        }

        public void TurnOffLight()
        {
            _logger.LogInformation("Turn off light");
        }
    }
}

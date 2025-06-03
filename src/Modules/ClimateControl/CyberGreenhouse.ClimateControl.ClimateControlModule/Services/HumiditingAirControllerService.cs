namespace CyberGreenhouse.ClimateControl.ClimateControlModule.Services
{
    public class HumiditingAirControllerService
    {
        private readonly ILogger<HumiditingAirControllerService> _logger;

        public HumiditingAirControllerService(ILogger<HumiditingAirControllerService> logger)
        {
            _logger = logger;
        }

        public void Increase()
        {
            _logger.LogInformation("Increasing air humidity");
        }

        public void Decrease()
        {
            _logger.LogInformation("Decrease air humidity");
        }
    }
}

namespace CyberGreenhouse.Irrigation.IrrigationControlModule.Controllers
{
    public class DripIrrigationControllerService
    {
        private readonly ILogger<DripIrrigationControllerService> _logger;

        public DripIrrigationControllerService(ILogger<DripIrrigationControllerService> logger)
        {
            _logger = logger;
        }

        public void Drip()
        {
            _logger.LogInformation("Drip water");
        }
    }
}

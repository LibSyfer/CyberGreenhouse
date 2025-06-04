using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Events.MaturityMonitoring;

namespace CyberGreenhouse.MaturityMonitoring.VisualInspectionModule.Services
{
    public class VisualInspectionBackgroundService : BackgroundService
    {
        private readonly ILogger<VisualInspectionBackgroundService> _logger;
        private readonly VisualInspectionDataService _visualInspectionDataService;
        private readonly IMessageBus _messageBus;

        public VisualInspectionBackgroundService(ILogger<VisualInspectionBackgroundService> logger, VisualInspectionDataService visualInspectionDataService, IMessageBus messageBus)
        {
            _logger = logger;
            _visualInspectionDataService = visualInspectionDataService;
            _messageBus = messageBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                var growingSolution = _visualInspectionDataService.GetInspectionSolution();
                if (growingSolution)
                {
                    _logger.LogInformation("Triggering the visual inspection trigger");
                    await _messageBus.SendAsync(ModuleNames.MaturityMonitoringControl, new VisualInspectionTriggeredEvent());
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}

using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Events.MaturityMonitoring;

namespace CyberGreenhouse.MaturityMonitoring.TimeControlModule
{
    public class TimeControlBackgroundService : BackgroundService
    {
        private readonly ILogger<TimeControlBackgroundService> _logger;
        private readonly IMessageBus _messageBus;

        private DateTime? _triggerTime;

        public void SetTriggerTime(TimeSpan timeSpan)
        {
            _logger.LogInformation("Trigger time set");
            _triggerTime = DateTime.UtcNow + timeSpan;
        }

        public TimeControlBackgroundService(ILogger<TimeControlBackgroundService> logger, IMessageBus messageBus)
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_triggerTime.HasValue)
                {
                    if (_triggerTime.Value <= DateTime.UtcNow)
                    {
                        _logger.LogInformation("Triggering the minimum growth time trigger");
                        await _messageBus.SendAsync(ModuleNames.MaturityMonitoringControl, new MinimalGrowthTimeTriggeredEvent());
                        _triggerTime = null;
                    }
                    else
                    {
                        _logger.LogInformation("Still early");
                    }
                }
                else
                {
                    _logger.LogInformation("Trigger not set");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}

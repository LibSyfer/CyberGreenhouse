using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.MaturityMonitoring;

namespace CyberGreenhouse.MaturityMonitoring.TimeControlModule.MessageHandlers
{
    public class StartTimeControlCommandHandler : IMessageBusHandler<StartTimeControlCommand>
    {
        private readonly ILogger<StartTimeControlCommandHandler> _logger;
        private readonly TimeControlBackgroundService _timeControlBackgroundService;

        public StartTimeControlCommandHandler(ILogger<StartTimeControlCommandHandler> logger, TimeControlBackgroundService timeControlBackgroundService)
        {
            _logger = logger;
            _timeControlBackgroundService = timeControlBackgroundService;
        }

        public Task Handle(StartTimeControlCommand message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Set trigger time");
            _timeControlBackgroundService.SetTriggerTime(TimeSpan.FromSeconds(message.MinGrowthSeconds));
            return Task.CompletedTask;
        }
    }
}

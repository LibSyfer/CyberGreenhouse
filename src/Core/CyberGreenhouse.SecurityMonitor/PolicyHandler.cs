using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

namespace CyberGreenhouse.SecurityMonitor
{
    public class PolicyHandler : IMonitorMessageBusHandler
    {
        private readonly ILogger<PolicyHandler> _logger;
        private readonly IMonitorMessageBus _monitorMessageBus;

        public PolicyHandler(ILogger<PolicyHandler> logger, IMonitorMessageBus monitorMessageBus)
        {
            _logger = logger;
            _monitorMessageBus = monitorMessageBus;
        }

        public async Task Handle(IDictionary<string, object?> metadata, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken = default)
        {
            var monitorHeaders = metadata.ReadMonitorHeaders();
            var authorizeAction = false;

            if (authorizeAction)
            {
                _logger.LogInformation($"Действие [Action: {monitorHeaders.ActionName}] [Source: {monitorHeaders.Source}] [Destination: {monitorHeaders.Destination}] разрешено политиками безопасности");

                await _monitorMessageBus.ResendAsync(monitorHeaders.Destination, monitorHeaders.ActionName, payload, cancellationToken);
            }
            else
            {
                _logger.LogWarning($"Действие [Action: {monitorHeaders.ActionName}] [Source: {monitorHeaders.Source}] [Destination: {monitorHeaders.Destination}] запрещено политиками безопасности");
            }
        }
    }
}

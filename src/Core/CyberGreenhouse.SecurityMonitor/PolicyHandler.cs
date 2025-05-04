using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Events;
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

            if (monitorHeaders.ActionName.Equals(nameof(GetPlantGrowingParamsCommand), StringComparison.OrdinalIgnoreCase)
                && monitorHeaders.Source.Equals(ModuleNames.MainControl, StringComparison.OrdinalIgnoreCase)
                && monitorHeaders.Destination.Equals(ModuleNames.PlantDataSignatureChecker, StringComparison.OrdinalIgnoreCase))
                authorizeAction = true;

            if (monitorHeaders.ActionName.Equals(nameof(GettedPlantGrowingParamsEvent), StringComparison.OrdinalIgnoreCase)
                && monitorHeaders.Source.Equals(ModuleNames.PlantDataSignatureChecker, StringComparison.OrdinalIgnoreCase)
                && monitorHeaders.Destination.Equals(ModuleNames.MainControl, StringComparison.OrdinalIgnoreCase))
                authorizeAction = true;

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

using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Events;
using CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule;
using CyberGreenhouse.MessageBus.Contracts.Events.LightingModule;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;
using static CyberGreenhouse.MessageBus.RabbitMQ.Extensions.MonitorHeadersExtensions;

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

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(GetPlantGrowingParamsCommand),
                sourceModule: ModuleNames.MainControl,
                destinationModule: ModuleNames.PlantDataSignatureChecker))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(GettedPlantGrowingParamsEvent),
                sourceModule: ModuleNames.PlantDataSignatureChecker,
                destinationModule: ModuleNames.MainControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(LightingLevelEvent),
                sourceModule: ModuleNames.LightSensorFilter,
                destinationModule: ModuleNames.LightingControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(AirHumidityEvent),
                sourceModule: ModuleNames.AirHumiditySensorFilter,
                destinationModule: ModuleNames.ClimateControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(AirTemperatureEvent),
                sourceModule: ModuleNames.AirTermoSensorFilter,
                destinationModule: ModuleNames.ClimateControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(WaterTemperatureEvent),
                sourceModule: ModuleNames.WaterTermoSensorFilter,
                destinationModule: ModuleNames.ClimateControl))
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

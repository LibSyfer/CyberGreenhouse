using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Commands.Harvesting;
using CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation;
using CyberGreenhouse.MessageBus.Contracts.Commands.MaturityMonitoring;
using CyberGreenhouse.MessageBus.Contracts.Commands.Planting;
using CyberGreenhouse.MessageBus.Contracts.Events;
using CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule;
using CyberGreenhouse.MessageBus.Contracts.Events.Harvesting;
using CyberGreenhouse.MessageBus.Contracts.Events.Irrigation;
using CyberGreenhouse.MessageBus.Contracts.Events.LightingModule;
using CyberGreenhouse.MessageBus.Contracts.Events.MaturityMonitoring;
using CyberGreenhouse.MessageBus.Contracts.Events.Planting;
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
                actionName: nameof(ReceivedPlantGrowingParamsEvent),
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

            // Irrigation
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(FertilizerPreparationCommand),
                sourceModule: ModuleNames.IrrigationControl,
                destinationModule: ModuleNames.NutrientCompositionControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(FertilizerPreparationCompleteEvent),
                sourceModule: ModuleNames.NutrientCompositionControl,
                destinationModule: ModuleNames.IrrigationControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(SoilHumidityEvent),
                sourceModule: ModuleNames.SoilHumiditySensorFilter,
                destinationModule: ModuleNames.IrrigationControl))
                authorizeAction = true;

            // MaturityMonitoring
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(StartTimeControlCommand),
                sourceModule: ModuleNames.MaturityMonitoringControl,
                destinationModule: ModuleNames.TimeControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(MinimalGrowthTimeTriggeredEvent),
                sourceModule: ModuleNames.TimeControl,
                destinationModule: ModuleNames.MaturityMonitoringControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(VisualInspectionTriggeredEvent),
                sourceModule: ModuleNames.VisualInspection,
                destinationModule: ModuleNames.MaturityMonitoringControl))
                authorizeAction = true;

            // Planting
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(StartPlantingCommand),
                sourceModule: ModuleNames.MainControl,
                destinationModule: ModuleNames.PlantingModule))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(PlantingCompleteEvent),
                sourceModule: ModuleNames.PlantingModule,
                destinationModule: ModuleNames.MainControl))
                authorizeAction = true;

            // Harvesting
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(StartHarvestingCommand),
                sourceModule: ModuleNames.MainControl,
                destinationModule: ModuleNames.HarvestingModule))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(HarvestingCompleteEvent),
                sourceModule: ModuleNames.HarvestingModule,
                destinationModule: ModuleNames.MainControl))
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

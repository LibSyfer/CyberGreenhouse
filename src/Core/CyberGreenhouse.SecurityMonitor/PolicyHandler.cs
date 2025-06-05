using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Commands.ClimateModule;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;
using CyberGreenhouse.MessageBus.Contracts.Commands.Harvesting;
using CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation;
using CyberGreenhouse.MessageBus.Contracts.Commands.LightingModule;
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

            // Form MainControlModule
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(StartGrowingCycleCommand),
                sourceModule: ModuleNames.MainControl,
                destinationModule: ModuleNames.GrowingCycleControlModule))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(SetLightingLevelCommand),
                sourceModule: ModuleNames.MainControl,
                destinationModule: ModuleNames.LightingControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(SetClimateParamsCommand),
                sourceModule: ModuleNames.MainControl,
                destinationModule: ModuleNames.ClimateControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(SetIrrigationParamsCommand),
                sourceModule: ModuleNames.MainControl,
                destinationModule: ModuleNames.IrrigationControl))
                authorizeAction = true;

            // From GrowingCycleControlModule
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(GetPlantGrowingParamsCommand),
                sourceModule: ModuleNames.GrowingCycleControlModule,
                destinationModule: ModuleNames.PlantDataSignatureChecker))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(SetupAllControlModulesCommand),
                sourceModule: ModuleNames.GrowingCycleControlModule,
                destinationModule: ModuleNames.MainControl))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(StartPlantingCommand),
                sourceModule: ModuleNames.GrowingCycleControlModule,
                destinationModule: ModuleNames.PlantingModule))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(StartHarvestingCommand),
                sourceModule: ModuleNames.GrowingCycleControlModule,
                destinationModule: ModuleNames.HarvestingModule))
                authorizeAction = true;

            // From PlantDataSignatureChecker
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(ReceivedPlantGrowingParamsEvent),
                sourceModule: ModuleNames.PlantDataSignatureChecker,
                destinationModule: ModuleNames.GrowingCycleControlModule))
                authorizeAction = true;

            // From LightingControl
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(LightingLevelEvent),
                sourceModule: ModuleNames.LightSensorFilter,
                destinationModule: ModuleNames.LightingControl))
                authorizeAction = true;

            // From ClimateControlModule
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

            // From Irrigation
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

            // From MaturityMonitoring
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

            // From Planting
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(PlantingCompleteEvent),
                sourceModule: ModuleNames.PlantingModule,
                destinationModule: ModuleNames.GrowingCycleControlModule))
                authorizeAction = true;

            // From Harvesting
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(HarvestingCompleteEvent),
                sourceModule: ModuleNames.HarvestingModule,
                destinationModule: ModuleNames.GrowingCycleControlModule))
                authorizeAction = true;

            // EmergencyStopModule
            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(AbordSystemCommand),
                sourceModule: ModuleNames.ClimateControl,
                destinationModule: ModuleNames.EmergencyStop))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(AbordSystemCommand),
                sourceModule: ModuleNames.ClimateControl,
                destinationModule: ModuleNames.EmergencyStop))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(AbordSystemCommand),
                sourceModule: ModuleNames.ClimateControl,
                destinationModule: ModuleNames.EmergencyStop))
                authorizeAction = true;

            if (monitorHeaders.AuthorizeAction(
                actionName: nameof(AbordSystemCommand),
                sourceModule: ModuleNames.ClimateControl,
                destinationModule: ModuleNames.EmergencyStop))
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

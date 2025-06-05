using CyberGreenhouse.Core;
using CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.MessageHandlers;
using CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.Services;
using CyberGreenhouse.MessageBus.Contracts.Commands.MaturityMonitoring;
using CyberGreenhouse.MessageBus.Contracts.Events.MaturityMonitoring;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<StateService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.MaturityMonitoringControl)
    .RegisterMessageHandler<StartMaturityMonitoringCommand, StartMaturityMonitoringCommandHandler>()
    .RegisterMessageHandler<MinimalGrowthTimeTriggeredEvent, MinimalGrowthTimeTriggeredEventHandler>()
    .RegisterMessageHandler<VisualInspectionTriggeredEvent, VisualInspectionTriggeredEventHandler>();

var host = builder.Build();
host.Run();

using CyberGreenhouse.Core;
using CyberGreenhouse.GrowingCycleControlModule.MessageHandlers;
using CyberGreenhouse.GrowingCycleControlModule.Services;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Events;
using CyberGreenhouse.MessageBus.Contracts.Events.Harvesting;
using CyberGreenhouse.MessageBus.Contracts.Events.MaturityMonitoring;
using CyberGreenhouse.MessageBus.Contracts.Events.Planting;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<StateService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.MainControl)
    .RegisterMessageHandler<StartGrowingCycleCommand, StartGrowingCycleCommandHandler>()
    .RegisterMessageHandler<ReceivedPlantGrowingParamsEvent, ReceivedPlantGrowingParamsEventHandler>()
    .RegisterMessageHandler<PlantingCompleteEvent, PlantingCompleteEventHandler>()
    .RegisterMessageHandler<MaturityCompletedEvent, MaturityCompletedEventHandler>()
    .RegisterMessageHandler<HarvestingCompleteEvent, HarvestingCompleteEventHandler>();

var host = builder.Build();
host.Run();

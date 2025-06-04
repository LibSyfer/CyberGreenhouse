using CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Controllers;
using CyberGreenhouse.Irrigation.NutrientCompositionControlModule.MessageHandlers;
using CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Services;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<StateService>();
builder.Services.AddSingleton<TankCleanerControllerService>();
builder.Services.AddSingleton<WaterSupplyControllerService>();
builder.Services.AddSingleton<FertilizerSupplyControllerService>();
builder.Services.AddSingleton<MixerControllerService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.NutrientCompositionControl)
    .RegisterMessageHandler<FertilizerPreparationCommand, FertilizerPreparationCommandHandler>();

var host = builder.Build();
host.Run();

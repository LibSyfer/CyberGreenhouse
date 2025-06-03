using CyberGreenhouse.Lighting.LightingControlModule.Controllers;
using CyberGreenhouse.Lighting.LightingControlModule.MessageHandlers;
using CyberGreenhouse.Lighting.LightingControlModule.Models;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands.LightingModule;
using CyberGreenhouse.MessageBus.Contracts.Events.LightingModule;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<LightingSettings>(builder.Configuration.GetSection(LightingSettings.Section));
builder.Services.AddSingleton<LightingControllerService>();
builder.Services.AddSingleton<RequiredLightingSettings>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.LightingControl)
    .RegisterMessageHandler<LightingLevelEvent, LightingLevelEventHandler>()
    .RegisterMessageHandler<SetLightingLevelCommand, SetLightingLevelCommandHandler>();

var host = builder.Build();
host.Run();

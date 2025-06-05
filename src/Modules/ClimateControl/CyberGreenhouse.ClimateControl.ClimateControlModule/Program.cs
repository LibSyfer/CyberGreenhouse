using CyberGreenhouse.ClimateControl.ClimateControlModule.Controllers;
using CyberGreenhouse.ClimateControl.ClimateControlModule.MessageHandlers;
using CyberGreenhouse.ClimateControl.ClimateControlModule.Models;
using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Contracts.Commands.ClimateModule;
using CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ClimateSettings>(builder.Configuration.GetSection(ClimateSettings.Section));
builder.Services.AddSingleton<RequiredClimateSettings>();
builder.Services.AddSingleton<HeatingAirControllerService>();
builder.Services.AddSingleton<FreezingAirControllerService>();
builder.Services.AddSingleton<HumiditingAirControllerService>();
builder.Services.AddSingleton<HeatingWaterControllerService>();
builder.Services.AddSingleton<FreezingWaterControllerService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.ClimateControl)
    .RegisterMessageHandler<SetClimateParamsCommand, SetClimateParamsCommandHandler>()
    .RegisterMessageHandler<AirTemperatureEvent, AirTemperatureEventHandler>()
    .RegisterMessageHandler<WaterTemperatureEvent, WaterTemperatureEventHandler>()
    .RegisterMessageHandler<AirHumidityEvent, AirHumidityEventHandler>();

var host = builder.Build();
host.Run();

using CyberGreenhouse.ClimateControl.ClimateControlModule.Models;
using CyberGreenhouse.ClimateControl.ClimateControlModule.Services;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ClimateSettings>(builder.Configuration.GetSection(ClimateSettings.Section));
builder.Services.AddSingleton<HeatingAirControllerService>();
builder.Services.AddSingleton<FreezingAirControllerService>();
builder.Services.AddSingleton<HumiditingAirControllerService>();
builder.Services.AddSingleton<HeatingWaterControllerService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.ClimateControl);

var host = builder.Build();
host.Run();

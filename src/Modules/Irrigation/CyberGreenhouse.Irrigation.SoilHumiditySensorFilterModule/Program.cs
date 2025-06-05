using CyberGreenhouse.Core;
using CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<SoilHumiditySensor>();
builder.Services.AddHostedService<SensorDataBackgroundService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.SoilHumiditySensorFilter);

var host = builder.Build();
host.Run();

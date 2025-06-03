using CyberGreenhouse.ClimateControl.AirHumiditySensorFilterModule;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<AirHumiditySensor>();
builder.Services.AddHostedService<SensorDataBackgroundService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.AirHumiditySensorFilter);

var host = builder.Build();
host.Run();

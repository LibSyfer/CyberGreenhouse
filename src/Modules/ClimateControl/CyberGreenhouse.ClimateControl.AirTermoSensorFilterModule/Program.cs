using CyberGreenhouse.ClimateControl.AirHumiditySensorFilterModule;
using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<AirTempreatureSensor>();
builder.Services.AddHostedService<SensorDataBackgroundService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.AirTermoSensorFilter);

var host = builder.Build();
host.Run();

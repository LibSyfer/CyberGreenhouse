using CyberGreenhouse.Core;
using CyberGreenhouse.Lighting.LightSensorFilterModule;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<LightingSensors>();
builder.Services.AddHostedService<SensorDataBackgroundService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.LightSensorFilter);

var host = builder.Build();
host.Run();

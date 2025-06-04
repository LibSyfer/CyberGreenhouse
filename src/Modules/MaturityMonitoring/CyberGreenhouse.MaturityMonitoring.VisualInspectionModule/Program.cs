using CyberGreenhouse.MaturityMonitoring.VisualInspectionModule.Services;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<VisualInspectionDataService>();
builder.Services.AddHostedService<VisualInspectionBackgroundService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.VisualInspection);

var host = builder.Build();
host.Run();

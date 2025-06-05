using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;
using CyberGreenhouse.SecurityMonitor;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMonitorRabbitMqMessageBus(builder.Configuration)
    .RegisterMonitorHandler<PolicyHandler>();

var host = builder.Build();
host.Run();

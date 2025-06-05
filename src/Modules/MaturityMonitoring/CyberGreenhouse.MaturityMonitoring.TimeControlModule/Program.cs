using CyberGreenhouse.Core;
using CyberGreenhouse.MaturityMonitoring.TimeControlModule;
using CyberGreenhouse.MaturityMonitoring.TimeControlModule.MessageHandlers;
using CyberGreenhouse.MessageBus.Contracts.Commands.MaturityMonitoring;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<TimeControlBackgroundService>();
builder.Services.AddHostedService(sp =>
{
    return sp.GetRequiredService<TimeControlBackgroundService>();
});
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.TimeControl)
    .RegisterMessageHandler<StartTimeControlCommand, StartTimeControlCommandHandler>();

var host = builder.Build();
host.Run();

using CyberGreenhouse.Core;
using CyberGreenhouse.EmergencyStopModule.MessageHandlers;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.EmergencyStop)
    .RegisterMessageHandler<AbordSystemCommand, AbordSystemCommandHandler>();

var host = builder.Build();
host.Run();

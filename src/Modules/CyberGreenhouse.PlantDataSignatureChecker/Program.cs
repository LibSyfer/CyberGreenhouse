using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;
using CyberGreenhouse.PlantDataSignatureChecker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.PlantDataSignatureChecker)
    .RegisterMessageHandler<GetPlantGrowingParamsCommand, GetPlantGrowingParamsCommandHandler>();

var host = builder.Build();
host.Run();

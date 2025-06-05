using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;
using CyberGreenhouse.PlantDataSignatureChecker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<GetPlantGrowingParamsCommandHandler>();

builder.Services.AddSingleton(_ =>
{
    return new SignatureService(builder.Configuration["SignatureService:Key"] ?? throw new ArgumentNullException("Signature service key not set"));
});

builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.PlantDataSignatureChecker)
    .RegisterMessageHandler<GetPlantGrowingParamsCommand, GetPlantGrowingParamsCommandHandler>();

var host = builder.Build();
host.Run();

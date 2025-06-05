using CyberGreenhouse.Core;
using CyberGreenhouse.Irrigation.IrrigationControlModule.Controllers;
using CyberGreenhouse.Irrigation.IrrigationControlModule.MessageHandlers;
using CyberGreenhouse.Irrigation.IrrigationControlModule.Models;
using CyberGreenhouse.Irrigation.IrrigationControlModule.Service;
using CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation;
using CyberGreenhouse.MessageBus.Contracts.Events.Irrigation;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<IrrigationSettings>(builder.Configuration.GetSection(IrrigationSettings.Section));
builder.Services.AddSingleton<StateService>();
builder.Services.AddSingleton<RequiredIrrigationSettings>();
builder.Services.AddSingleton<DripIrrigationControllerService>();

builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.IrrigationControl)
    .RegisterMessageHandler<SetIrrigationParamsCommand, SetIrrigationParamsCommandHandler>()
    .RegisterMessageHandler<SoilHumidityEvent, SoilHumidityEventHandler>()
    .RegisterMessageHandler<FertilizerPreparationCompleteEvent, FertilizerPreparationCompleteEventHandler>();


var host = builder.Build();
host.Run();

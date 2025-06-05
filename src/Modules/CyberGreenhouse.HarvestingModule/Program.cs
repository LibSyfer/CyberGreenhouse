using CyberGreenhouse.HarvestingModule.MessageHandlers;
using CyberGreenhouse.HarvestingModule.Models;
using CyberGreenhouse.HarvestingModule.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands.Harvesting;
using CyberGreenhouse.MessageBus.Contracts.Events.Harvesting;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<HarvestingSettings>(builder.Configuration.GetSection(HarvestingSettings.Section));
builder.Services.AddSingleton<StateService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.HarvestingModule)
    .RegisterMessageHandler<StartHarvestingCommand, StartHarvestingCommandHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/status", (StateService stateService) =>
{
    return stateService.CurrentState switch
    {
        HarvestingStatus.NotInitiated => Results.BadRequest(new
        {
            Status = HarvestingStatus.NotInitiated.ToString(),
            Message = "Harvesting readiness is not initiated"
        }),
        HarvestingStatus.Ready => Results.Ok(new
        {
            Status = HarvestingStatus.Ready.ToString(),
            Message = "Ready for harvesting"
        }),
        HarvestingStatus.Harvesting => Results.Ok(new
        {
            Code = HarvestingStatus.Harvesting.ToString(),
            Message = "Harvesting in proccess"
        }),
        HarvestingStatus.Complete => Results.Ok(new
        {
            Code = HarvestingStatus.Complete.ToString(),
            Message = "Harvesting completed"
        }),
        _ => Results.BadRequest(new
        {
            Code = "Unknown",
            Message = "Unknown state"
        })
    };
})
.WithName("StatusHarvesting")
.WithOpenApi();

app.MapPost("/start-harvesting", (StateService stateService) =>
{
    if (stateService.CurrentState is not HarvestingStatus.Ready)
    {
        return Results.BadRequest($"Cannot start harvesting, state must be {HarvestingStatus.Ready.ToString()}, but in state: {stateService.CurrentState.ToString()}");
    }

    stateService.CurrentState = HarvestingStatus.Harvesting;
    return Results.Ok("Harvesting start");
})
.WithName("StartHarvesting")
.WithOpenApi();

app.MapPost("/finish-harvesting", async (StateService stateService, IMessageBus messageBus, CancellationToken cancellationToken) =>
{
    if (stateService.CurrentState is not HarvestingStatus.Harvesting)
    {
        return Results.BadRequest($"Cannot finish harvesting, state must be {HarvestingStatus.Harvesting.ToString()}, but in state: {stateService.CurrentState.ToString()}");
    }

    stateService.CurrentState = HarvestingStatus.Complete;

    await messageBus.SendAsync(ModuleNames.MainControl, new HarvestingCompleteEvent());

    return Results.Ok("Harvesting completed");
})
.WithName("FinishHarvesting")
.WithOpenApi();

app.Run();
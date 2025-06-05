using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands.Planting;
using CyberGreenhouse.MessageBus.Contracts.Events.Planting;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;
using CyberGreenhouse.PlantingModule.MessageHandlers;
using CyberGreenhouse.PlantingModule.Models;
using CyberGreenhouse.PlantingModule.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<PlantingSettings>(builder.Configuration.GetSection(PlantingSettings.Section));
builder.Services.AddSingleton<StateService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.PlantingModule)
    .RegisterMessageHandler<StartPlantingCommand, StartPlantingCommandHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/status", (StateService stateService) =>
{
    return stateService.CurrentState switch
    {
        PlantingStatus.NotInitiated => Results.BadRequest(new
        {
            Status = PlantingStatus.NotInitiated.ToString(),
            Message = "Planting readiness is not initiated"
        }),
        PlantingStatus.Ready => Results.Ok(new
        {
            Status = PlantingStatus.Ready.ToString(),
            Message = "Ready for planting"
        }),
        PlantingStatus.Planting => Results.Ok(new
        {
            Code = PlantingStatus.Planting.ToString(),
            Message = "Planting in proccess"
        }),
        PlantingStatus.Complete => Results.Ok(new
        {
            Code = PlantingStatus.Complete.ToString(),
            Message = "Planting completed"
        }),
        _ => Results.BadRequest(new
        {
            Code = "Unknown",
            Message = "Unknown state"
        })
    };
})
.WithName("StatusPlanting")
.WithOpenApi();

app.MapPost("/start-planting", (StateService stateService) =>
{
    if (stateService.CurrentState is not PlantingStatus.Ready)
    {
        return Results.BadRequest($"Cannot start planting, state must be {PlantingStatus.Ready.ToString()}, but in state: {stateService.CurrentState.ToString()}");
    }

    stateService.CurrentState = PlantingStatus.Planting;
    return Results.Ok("Planting start");
})
.WithName("StartPlanting")
.WithOpenApi();

app.MapPost("/finish-planting", async (StateService stateService, IMessageBus messageBus, CancellationToken cancellationToken) =>
{
    if (stateService.CurrentState is not PlantingStatus.Planting)
    {
        return Results.BadRequest($"Cannot finish planting, state must be {PlantingStatus.Planting.ToString()}, but in state: {stateService.CurrentState.ToString()}");
    }

    stateService.CurrentState = PlantingStatus.Complete;

    await messageBus.SendAsync(ModuleNames.MainControl, new PlantingCompleteEvent());

    return Results.Ok("Planting completed");
})
.WithName("FinishPlanting")
.WithOpenApi();

app.Run();

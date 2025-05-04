using CyberGreenhouse.MainControl;
using CyberGreenhouse.MainControl.MessageHandlers;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Events;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GrowingService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.MainControl)
    .RegisterMessageHandler<GettedPlantGrowingParamsEvent, GettedPlantGrowingParamsEventHandler>()
    .RegisterMessageHandler<PlantingFinishedEvent, PlantingFinishedEventHandler>()
    .RegisterMessageHandler<PlantReadyEvent, PlantReadyEventHandler>()
    .RegisterMessageHandler<HarvestingFinishedEvent, HarvestingFinishedEventHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/status", (GrowingService growingService) =>
{
    return Results.Ok(new
    {
        growingService.IsBuzy
    });
})
.WithDisplayName("status")
.WithOpenApi();

app.MapPost("/grow", async (GrowingService growingService, Guid paramsId, CancellationToken cancellationToken) =>
{
    var isSuccess = await growingService.GrowAsync(paramsId, cancellationToken);
    if (!isSuccess)
    {
        return Results.BadRequest();
    }
    return Results.Ok();
})
.WithDisplayName("grow")
.WithOpenApi();

app.Run();
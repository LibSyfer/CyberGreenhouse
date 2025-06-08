using CyberGreenhouse.Core;
using CyberGreenhouse.MainControl.MessageHandlers;
using CyberGreenhouse.MainControl.Services;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;
using CyberGreenhouse.MessageBus.Contracts.Events;
using CyberGreenhouse.MessageBus.Extensions;
using CyberGreenhouse.MessageBus.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<StateService>();
builder.Services.AddClientRabbitMqMessageBus(builder.Configuration, ModuleNames.MainControl)
    .RegisterMessageHandler<SetupAllControlModulesCommand, SetupAllControlModulesCommandHandler>()
    .RegisterMessageHandler<GrowingCompleteEvent, GrowingCompleteEventHandler>()
    .RegisterMessageHandler<AbordSystemCommand, AbordSystemCommandHandler>();

var HasExploit = builder.Configuration.GetValue<bool>("HasExploit");
if (HasExploit)
{
    builder.Services.AddHostedService<ExploitService>();
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/status", (StateService stateService) =>
{
    return stateService.CurrentState switch
    {
        MainControlStatus.Ready => Results.Ok(new
        {
            Status = MainControlStatus.Ready.ToString(),
            Message = "Ready for planting"
        }),
        MainControlStatus.Growing => Results.Ok(new
        {
            Code = MainControlStatus.Growing.ToString(),
            Message = "Growing in proccess"
        }),
        MainControlStatus.Completed => Results.Ok(new
        {
            Code = MainControlStatus.Completed.ToString(),
            Message = "Growing completed"
        }),
        MainControlStatus.Aborded => Results.Ok(new
        {
            Code = MainControlStatus.Aborded.ToString(),
            Message = $"[{stateService.ErrorModule}] {stateService.Error}"
        }),
        _ => Results.BadRequest(new
        {
            Code = "Unknown",
            Message = "Unknown state"
        })
    };
})
.WithDisplayName("status")
.WithOpenApi();

app.MapPost("/grow", async (IMessageBus messageBus, Guid paramsId, StateService stateService, CancellationToken cancellationToken) =>
{
    if (stateService.CurrentState is not MainControlStatus.Ready)
    {
        return Results.BadRequest($"Cannot start growing, state must be {MainControlStatus.Ready.ToString()}, but in state: {stateService.CurrentState.ToString()}");
    }

    stateService.CurrentState = MainControlStatus.Growing;
    await messageBus.SendAsync(ModuleNames.GrowingCycleControlModule, new StartGrowingCycleCommand
    {
        ParamId = paramsId,
    });

    return Results.Ok();
})
.WithDisplayName("grow")
.WithOpenApi();

app.Run();
using CyberGreenhouse.Core;
using CyberGreenhouse.External.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(_ =>
{
    return new SignatureService(builder.Configuration["SignatureService:Key"] ?? throw new ArgumentNullException("Signature service key not set"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ResponseSigningMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("TomatoDatabase");

app.MapGet("/tomatos", () =>
{
    logger.LogInformation("[GET] /tomatos");

    return TomatoDatabase.Tomatos;
});

app.MapGet("/tomatos/growing-params", () =>
{
    logger.LogInformation($"[GET] /tomatos/growing-params");

    var growingParamsList = TomatoDatabase.Params.ToList();
    return Results.Ok(growingParamsList);
});

app.MapGet("/tomatos/growing-params/{paramsId}", (Guid paramsId) =>
{
    logger.LogInformation($"[GET] /tomatos/growing-params/{paramsId}");
    var growingParams = TomatoDatabase.Params.Where(e => e.Id == paramsId).FirstOrDefault();
    if (growingParams == null)
    {
        logger.LogInformation($"Not found");
        return Results.NotFound();
    }

    return Results.Ok(growingParams);
});

app.Run();

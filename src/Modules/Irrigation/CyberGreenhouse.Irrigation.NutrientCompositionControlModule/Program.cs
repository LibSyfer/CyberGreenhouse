using CyberGreenhouse.Irrigation.NutrientCompositionControlModule;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

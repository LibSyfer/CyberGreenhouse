using CyberGreenhouse.ClimateControl.ClimateHumiditySensorFilterModule;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

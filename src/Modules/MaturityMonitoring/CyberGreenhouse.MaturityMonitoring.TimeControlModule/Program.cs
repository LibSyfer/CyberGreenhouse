using CyberGreenhouse.MaturityMonitoring.TimeControlModule;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

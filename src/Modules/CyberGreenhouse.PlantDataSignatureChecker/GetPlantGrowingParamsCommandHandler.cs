using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;
using CyberGreenhouse.MessageBus.Contracts.Events;
using System.Text.Json;

namespace CyberGreenhouse.PlantDataSignatureChecker
{
    public class GetPlantGrowingParamsCommandHandler : IMessageBusHandler<GetPlantGrowingParamsCommand>
    {
        private readonly ILogger<GetPlantGrowingParamsCommandHandler> _logger;
        private readonly IMessageBus _messageBus;
        private readonly SignatureService _signatureService;
        private readonly HttpClient _httpClient;
        private readonly string _connectionString;

        public GetPlantGrowingParamsCommandHandler(ILogger<GetPlantGrowingParamsCommandHandler> logger, IMessageBus messageBus, SignatureService signatureService, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _messageBus = messageBus;
            _signatureService = signatureService;
            _httpClient = httpClient;
            _connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException("Database connsection string is null");
        }

        public async Task Handle(GetPlantGrowingParamsCommand message, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"{_connectionString}/tomatos/growing-params/{message.ParamId}", cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.Headers.TryGetValues("X-Api-Signature", out var signatures))
            {
                _logger.LogCritical("Signature of data from database not exists. Send abord command...");

                await _messageBus.SendAsync(ModuleNames.MainControl, new AbordCommand
                {
                    Message = "Signature of data from database not exists"
                },
                cancellationToken);

                return;
            }

            if (!_signatureService.VerifyData(content, signatures.First()))
            {
                _logger.LogCritical("Wrong signature of data from database. Send abord command...");

                await _messageBus.SendAsync(ModuleNames.MainControl, new AbordCommand
                {
                    Message = "Wrong signature of data from database"
                },
                cancellationToken);

                return;
            }

            var growingParams = JsonSerializer.Deserialize<PlantGrowingParams>(content);
            if (growingParams is null)
            {
                _logger.LogCritical("Data cannot be deserialize. Send abord command...");

                await _messageBus.SendAsync(ModuleNames.MainControl, new AbordCommand
                {
                    Message = "Data cannot be deserialize"
                },
                cancellationToken);

                return;
            }

            _logger.LogInformation("Send getted growing params");
            await _messageBus.SendAsync(ModuleNames.MainControl, new GettedPlantGrowingParamsEvent
            {
                ParamId = growingParams.Id,
                TomatoId = growingParams.TomatoId,
                LightIntensity = growingParams.LightIntensity,
                LightDuration = growingParams.LightDuration,
                TemperatureDay = growingParams.TemperatureDay,
                TemperatureNight = growingParams.TemperatureNight,
                HumidityLevel = growingParams.HumidityLevel,
                WateringFrequency = growingParams.WateringFrequency,
                FertilizerType = growingParams.FertilizerType
            },
            cancellationToken);
        }
    }
}

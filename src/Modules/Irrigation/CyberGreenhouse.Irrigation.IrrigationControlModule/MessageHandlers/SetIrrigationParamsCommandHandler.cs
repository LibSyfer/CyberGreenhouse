using CyberGreenhouse.Irrigation.IrrigationControlModule.Models;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands.Irrigation;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.Irrigation.IrrigationControlModule.MessageHandlers
{
    public class SetIrrigationParamsCommandHandler : IMessageBusHandler<SetIrrigationParamsCommand>
    {
        private readonly ILogger<SetIrrigationParamsCommandHandler> _logger;
        private readonly IrrigationSettings _irrigationSettings;
        private readonly RequiredIrrigationSettings _requiredIrrigationSettings;
        private readonly IMessageBus _messageBus;

        public SetIrrigationParamsCommandHandler(ILogger<SetIrrigationParamsCommandHandler> logger,
            IOptions<IrrigationSettings> irrigationSettingsOpt,
            RequiredIrrigationSettings requiredIrrigationSettings,
            IMessageBus messageBus
            )
        {
            _logger = logger;
            _irrigationSettings = irrigationSettingsOpt.Value;
            _requiredIrrigationSettings = requiredIrrigationSettings;
            _messageBus = messageBus;
        }

        public async Task Handle(SetIrrigationParamsCommand message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Set required irrigation settings. Soil humidity: {@SoilHumidity}. Fertilizer concentration Ppm: {@FertilizerConcentrationPpm}", message.SoilHumidity, message.FertilizerConcentrationPpm);
            if (message.SoilHumidity > _irrigationSettings.MaxSoilHumidity || message.SoilHumidity < _irrigationSettings.MinSoilHumidity)
            {
                _logger.LogWarning("Soil humidity out of range: {@SoilHumidity}, required range: {@MinSoilHumidity} - {@MaxSoilHumidity}.", message.SoilHumidity, _irrigationSettings.MinSoilHumidity, _irrigationSettings.MaxSoilHumidity);
            }
            else
            {
                _requiredIrrigationSettings.RequiredSoilHumidity = message.SoilHumidity;
            }

            if (message.FertilizerConcentrationPpm > _irrigationSettings.MaxFertilizerConcentrationPpm || message.FertilizerConcentrationPpm < _irrigationSettings.MinFertilizerConcentrationPpm)
            {
                _logger.LogWarning("Fertilizer concentration Ppm out of range: {@FertilizerConcentrationPpm}, required range: {@MinFertilizerConcentrationPpm} - {@MaxFertilizerConcentrationPpm}.", message.FertilizerConcentrationPpm, _irrigationSettings.MinFertilizerConcentrationPpm, _irrigationSettings.MaxFertilizerConcentrationPpm);
            }
            else
            {
                _requiredIrrigationSettings.RequiredFertilizerConcentrationPpm = message.FertilizerConcentrationPpm;
            }

            _logger.LogInformation($"Send preparation fertilizer command to {ModuleNames.NutrientCompositionControl}");
            await _messageBus.SendAsync(ModuleNames.NutrientCompositionControl, new FertilizerPreparationCommand
            {
                FertilizerConcentrationPpm = message.FertilizerConcentrationPpm
            });
        }
    }
}

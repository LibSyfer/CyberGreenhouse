using CyberGreenhouse.Core;
using CyberGreenhouse.Irrigation.IrrigationControlModule.Controllers;
using CyberGreenhouse.Irrigation.IrrigationControlModule.Models;
using CyberGreenhouse.Irrigation.IrrigationControlModule.Service;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Commands.EmergencyStopModule;
using CyberGreenhouse.MessageBus.Contracts.Events.Irrigation;
using Microsoft.Extensions.Options;

namespace CyberGreenhouse.Irrigation.IrrigationControlModule.MessageHandlers
{
    public class SoilHumidityEventHandler : IMessageBusHandler<SoilHumidityEvent>
    {
        private readonly ILogger<SoilHumidityEventHandler> _logger;
        private readonly StateService _stateService;
        private readonly IrrigationSettings _irrigationSettings;
        private readonly RequiredIrrigationSettings _requiredIrrigationSettings;
        private readonly DripIrrigationControllerService _dripIrrigationControllerService;
        private readonly IMessageBus _messageBus;

        public SoilHumidityEventHandler(ILogger<SoilHumidityEventHandler> logger,
            StateService stateService,
            IOptions<IrrigationSettings> irrigationSettings,
            RequiredIrrigationSettings requiredIrrigationSettings,
            DripIrrigationControllerService dripIrrigationControllerService,
            IMessageBus messageBus)
        {
            _logger = logger;
            _stateService = stateService;
            _irrigationSettings = irrigationSettings.Value;
            _requiredIrrigationSettings = requiredIrrigationSettings;
            _dripIrrigationControllerService = dripIrrigationControllerService;
            _messageBus = messageBus;
        }

        public async Task Handle(SoilHumidityEvent message, CancellationToken cancellationToken = default)
        {
            if (_stateService.CurrentState is not IrrigationStatus.Work) return;

            if (message.CurrentSoilHumidity < (_requiredIrrigationSettings.RequiredSoilHumidity - _irrigationSettings.DeviationSoilHumidity)
                || message.CurrentSoilHumidity > (_requiredIrrigationSettings.RequiredSoilHumidity + _irrigationSettings.DeviationSoilHumidity))
            {
                if (_requiredIrrigationSettings.CurrentSoilHumidityStabilizationAttempt == _requiredIrrigationSettings.StabilizationAttempts)
                {
                    await _messageBus.SendAsync(ModuleNames.EmergencyStop, new AbordSystemCommand
                    {
                        ModuleName = ModuleNames.IrrigationControl,
                        ErrorMessage = "Cannot stabilize soil humidity"
                    });

                    return;
                }

                if (message.CurrentSoilHumidity < (_requiredIrrigationSettings.RequiredSoilHumidity - _irrigationSettings.DeviationSoilHumidity))
                {
                    if (message.CurrentSoilHumidity < _irrigationSettings.MinSoilHumidity)
                    {
                        _logger.LogError("Dangerous soil humidity");
                    }

                    _requiredIrrigationSettings.CurrentSoilHumidityStabilizationAttempt++;
                    _dripIrrigationControllerService.Drip();
                }
            }
        }
    }
}

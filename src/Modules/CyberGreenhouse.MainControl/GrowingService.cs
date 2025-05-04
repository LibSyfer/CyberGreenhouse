using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Commands;

namespace CyberGreenhouse.MainControl
{
    public class GrowingService
    {
        private readonly ILogger<GrowingService> _logger;
        private readonly IMessageBus _messageBus;

        private PlantGrowingParams? _growingParams;
        private readonly object _growingParamsLock = new object();

        private bool _isBuzy = false;
        private readonly object _isBuzyLock = new object();

        public GrowingService(ILogger<GrowingService> logger, IMessageBus messageBus)
        {
            _logger = logger;
            _messageBus = messageBus;
        }

        public PlantGrowingParams? GrowingParams
        {
            get
            {
                lock (_growingParamsLock)
                {
                    return _growingParams;
                }
            }
            set
            {
                lock (_growingParamsLock)
                {
                    _growingParams = value;
                }
            }
        }

        public bool IsBuzy
        {
            get
            {
                lock (_isBuzyLock)
                {
                    return _isBuzy;
                }
            }
        }

        public async Task<bool> GrowAsync(Guid paramsId, CancellationToken cancellationToken)
        {
            if (IsBuzy) return false;

            _logger.LogInformation("Start growing. Getting growing params");
            await _messageBus.SendAsync(ModuleNames.PlantDataSignatureChecker, new GetPlantGrowingParamsCommand
            {
                ParamId = paramsId,
            },
            cancellationToken);

            return true;
        }

        public bool FinishGrow()
        {
            if (!IsBuzy)
            {
                return false;
            }

            lock (_isBuzyLock)
            {
                _isBuzy = false;
            }

            return true;
        }
    }
}

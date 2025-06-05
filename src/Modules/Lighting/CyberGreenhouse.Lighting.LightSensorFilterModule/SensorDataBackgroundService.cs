using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events.LightingModule;

namespace CyberGreenhouse.Lighting.LightSensorFilterModule
{
    public class SensorDataBackgroundService : BackgroundService
    {
        private readonly LightingSensors _sensors;
        private readonly IMessageBus _messageBus;
        private readonly int _sensorsCount;

        public SensorDataBackgroundService(LightingSensors sensors, IMessageBus messageBus, int sensorsCount = 3)
        {
            _sensors = sensors;
            _messageBus = messageBus;
            _sensorsCount = sensorsCount;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var ligthData = new List<double>();
                foreach (var _ in Enumerable.Range(1, _sensorsCount))
                    ligthData.Add(_sensors.GetSensorData());

                var currentLightIntensity = AvarageFilter(ligthData);

                await _messageBus.SendAsync(ModuleNames.LightingControl, new LightingLevelEvent
                {
                    LightIntensity = currentLightIntensity
                },
                stoppingToken);

                await Task.Delay(5000);
            }
        }

        private double AvarageFilter(List<double> lightData)
        {
            double sum = 0;
            foreach (var value in lightData)
            {
                sum += value;
            }

            return sum / lightData.Count;
        }
    }
}

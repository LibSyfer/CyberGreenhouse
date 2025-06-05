using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule;

namespace CyberGreenhouse.ClimateControl.AirHumiditySensorFilterModule
{
    public class SensorDataBackgroundService : BackgroundService
    {
        private readonly AirHumiditySensor _sensors;
        private readonly IMessageBus _messageBus;
        private readonly int _sensorsCount;

        public SensorDataBackgroundService(AirHumiditySensor sensors, IMessageBus messageBus, int sensorsCount = 3)
        {
            _sensors = sensors;
            _messageBus = messageBus;
            _sensorsCount = sensorsCount;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var humidityeData = new List<double>();
                foreach (var _ in Enumerable.Range(1, _sensorsCount))
                    humidityeData.Add(_sensors.GetSensorData());

                var currentHumidity = AvarageFilter(humidityeData);

                await _messageBus.SendAsync(ModuleNames.ClimateControl, new AirHumidityEvent
                {
                    Humidity = currentHumidity
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

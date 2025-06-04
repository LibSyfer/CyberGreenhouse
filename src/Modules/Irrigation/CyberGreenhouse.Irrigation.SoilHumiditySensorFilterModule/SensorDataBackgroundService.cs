using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Common;
using CyberGreenhouse.MessageBus.Contracts.Events.Irrigation;

namespace CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule
{
    public class SensorDataBackgroundService : BackgroundService
    {
        private readonly SoilHumiditySensor _sensors;
        private readonly IMessageBus _messageBus;
        private readonly int _sensorsCount;

        public SensorDataBackgroundService(SoilHumiditySensor sensors, IMessageBus messageBus, int sensorsCount = 3)
        {
            _sensors = sensors;
            _messageBus = messageBus;
            _sensorsCount = sensorsCount;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var soilHumidityData = new List<double>();
                foreach (var _ in Enumerable.Range(1, _sensorsCount))
                    soilHumidityData.Add(_sensors.GetSensorData());

                var currentSoilHumidity = AvarageFilter(soilHumidityData);

                await _messageBus.SendAsync(ModuleNames.IrrigationControl, new SoilHumidityEvent
                {
                    CurrentSoilHumidity = currentSoilHumidity
                },
                stoppingToken);

                await Task.Delay(5000);
            }
        }

        private double AvarageFilter(List<double> data)
        {
            double sum = 0;
            foreach (var value in data)
            {
                sum += value;
            }

            return sum / data.Count;
        }
    }
}

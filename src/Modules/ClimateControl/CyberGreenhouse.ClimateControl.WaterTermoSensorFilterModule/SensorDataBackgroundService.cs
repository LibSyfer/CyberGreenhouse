using CyberGreenhouse.Core;
using CyberGreenhouse.MessageBus.Abstractions;
using CyberGreenhouse.MessageBus.Contracts.Events.ClimateModule;

namespace CyberGreenhouse.ClimateControl.AirHumiditySensorFilterModule
{
    public class SensorDataBackgroundService : BackgroundService
    {
        private readonly WaterTempreatureSensor _sensors;
        private readonly IMessageBus _messageBus;
        private readonly int _sensorsCount;

        public SensorDataBackgroundService(WaterTempreatureSensor sensors, IMessageBus messageBus, int sensorsCount = 3)
        {
            _sensors = sensors;
            _messageBus = messageBus;
            _sensorsCount = sensorsCount;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tempreatureData = new List<double>();
                foreach (var _ in Enumerable.Range(1, _sensorsCount))
                    tempreatureData.Add(_sensors.GetSensorData());

                var currentTempreature = AvarageFilter(tempreatureData);

                await _messageBus.SendAsync(ModuleNames.ClimateControl, new WaterTemperatureEvent
                {
                    Temperature = currentTempreature
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

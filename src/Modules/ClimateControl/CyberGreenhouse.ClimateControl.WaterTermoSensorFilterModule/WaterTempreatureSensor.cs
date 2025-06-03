namespace CyberGreenhouse.ClimateControl.AirHumiditySensorFilterModule
{
    public class WaterTempreatureSensor
    {
        private readonly Random _random = new Random();
        private readonly List<double> SensorsData;

        public WaterTempreatureSensor()
        {
            SensorsData = new List<double>()
            {
                0,
                3.6,
                10.4,
                20.1,
                21.4,
                22.9,
                23.2,
                24.7,
                25.5,
                26.7,
                27.3,
                28.4,
                29.1,
                60.3,
                80.5
            };
        }

        public double GetSensorData()
        {
            return SensorsData[_random.Next(0, SensorsData.Count - 1)];
        }
    }
}

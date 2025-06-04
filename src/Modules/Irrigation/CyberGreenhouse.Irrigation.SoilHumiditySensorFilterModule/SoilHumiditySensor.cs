namespace CyberGreenhouse.Irrigation.SoilHumiditySensorFilterModule
{
    public class SoilHumiditySensor
    {
        private readonly Random _random = new Random();
        private readonly List<double> SensorsData;

        public SoilHumiditySensor()
        {
            SensorsData = new List<double>()
            {
                0,
                3.6,
                10.4,
                20.1,
                28.4,
                29.1,
                60.3,
                61.6,
                62.5,
                63.9,
                64.1,
                65.8,
                66.7,
                67.8,
                68.4,
                69.2,
                70.1,
                71.3,
                72.5,
                73.4,
                74.4,
                75.8,
                76.1,
                77.6,
                78.3,
                79.7,
                93.2,
                96.4,
                99.9
            };
        }

        public double GetSensorData()
        {
            return SensorsData[_random.Next(0, SensorsData.Count - 1)];
        }
    }
}

namespace CyberGreenhouse.ClimateControl.AirHumiditySensorFilterModule
{
    public class AirTempreatureSensor
    {
        private readonly Random _random = new Random();
        private readonly List<double> SensorsData;

        public AirTempreatureSensor()
        {
            SensorsData = new List<double>()
            {
                0,
                2.8,
                2.9,
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
                30.6,
                31.3,
                32.9,
                33.7,
                34.5,
                35.2,
                36.9,
                37.6,
                38.3,
                39.5,
                60.3,
                80.5,
                100.1
            };
        }

        public double GetSensorData()
        {
            return SensorsData[_random.Next(0, SensorsData.Count - 1)];
        }
    }
}

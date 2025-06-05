namespace CyberGreenhouse.ClimateControl.AirHumiditySensorFilterModule
{
    public class AirHumiditySensor
    {
        private readonly Random _random = new Random();
        private readonly List<double> SensorsData;

        public AirHumiditySensor()
        {
            SensorsData = new List<double>()
            {
                10.0,
                31.3,
                38.7,
                40.1,
                49.7,
                50.5,
                55.3,
                56.1,
                57.9,
                58.2,
                58.9,
                60.4,
                61.9,
                62.5,
                63.7,
                64.6,
                65.9,
                66.1,
                66.3,
                67.1,
                69.6,
                69.2,
                70.0,
                75.1,
                80.2,
                100.1
            };
        }

        public double GetSensorData()
        {
            return SensorsData[_random.Next(0, SensorsData.Count - 1)];
        }
    }
}

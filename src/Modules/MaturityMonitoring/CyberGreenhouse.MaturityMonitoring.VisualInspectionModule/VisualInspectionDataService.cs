namespace CyberGreenhouse.ClimateControl.AirHumiditySensorFilterModule
{
    public class VisualInspectionDataService
    {
        private readonly Random _random = new Random();
        private int _persentOfTrue;

        public VisualInspectionDataService(int persentOfTrue = 10)
        {
            _persentOfTrue = persentOfTrue;
        }

        public bool GetInspectionSolution()
        {
            return _random.Next(0, 100) < _persentOfTrue;
        }
    }
}

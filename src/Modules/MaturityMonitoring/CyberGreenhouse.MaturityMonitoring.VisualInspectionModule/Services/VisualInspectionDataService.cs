namespace CyberGreenhouse.MaturityMonitoring.VisualInspectionModule.Services
{
    public class VisualInspectionDataService
    {
        private readonly Random _random = new Random();
        private int _persentOfTrue;

        public VisualInspectionDataService(int persentOfTrue = 30)
        {
            _persentOfTrue = persentOfTrue;
        }

        public bool GetInspectionSolution()
        {
            return _random.Next(0, 100) < _persentOfTrue;
        }
    }
}

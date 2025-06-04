namespace CyberGreenhouse.Irrigation.NutrientCompositionControlModule.Services
{
    public class TankParams
    {
        public TankState CurrentState { get; set; }
    }

    public enum TankState
    {
        NotWork,
        Cleaning,
        SupplyWater,
        SupplyFertilizer,
        Mixing
    }
}

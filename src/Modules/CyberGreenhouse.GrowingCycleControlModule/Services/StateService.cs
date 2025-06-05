using CyberGreenhouse.Core;

namespace CyberGreenhouse.GrowingCycleControlModule.Services
{
    public class StateService
    {
        public GrowingCycleStatus CurrentState { get; set; }

        public PlantGrowingParams? PlantGrowingParams { get; set; }

        public StateService()
        {
            CurrentState = GrowingCycleStatus.NotWork;
        }
    }

    public enum GrowingCycleStatus
    {
        NotWork,
        Planting,
        GettingParams,
        Growing,
        Harvesting,
        Complete,
        Aborded
    }
}

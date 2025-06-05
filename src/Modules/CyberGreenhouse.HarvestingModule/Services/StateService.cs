namespace CyberGreenhouse.HarvestingModule.Services
{
    public class StateService
    {
        public HarvestingStatus CurrentState { get; set; }

        public StateService()
        {
            CurrentState = HarvestingStatus.NotInitiated;
        }
    }

    public enum HarvestingStatus
    {
        NotInitiated,
        Ready,
        Harvesting,
        Complete
    }
}

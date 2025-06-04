namespace CyberGreenhouse.HarvestingModule.Services
{
    public class StateService
    {
        public HarvestingStatus CurrentState { get; set; }

        public StateService()
        {
            CurrentState = HarvestingStatus.Ready;
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

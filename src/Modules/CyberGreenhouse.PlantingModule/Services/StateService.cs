namespace CyberGreenhouse.PlantingModule.Services
{
    public class StateService
    {
        public PlantingStatus CurrentState { get; set; }

        public StateService()
        {
            CurrentState = PlantingStatus.Ready;
        }
    }

    public enum PlantingStatus
    {
        NotInitiated,
        Ready,
        Planting,
        Complete
    }
}

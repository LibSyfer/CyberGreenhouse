namespace CyberGreenhouse.PlantingModule.Services
{
    public class StateService
    {
        public PlantingStatus CurrentState { get; set; }

        public StateService()
        {
            CurrentState = PlantingStatus.NotInitiated;
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

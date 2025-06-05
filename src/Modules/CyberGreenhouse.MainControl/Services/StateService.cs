namespace CyberGreenhouse.MainControl.Services
{
    public class StateService
    {
        public MainControlStatus CurrentState { get; set; }

        public StateService()
        {
            CurrentState = MainControlStatus.Ready;
        }
    }

    public enum MainControlStatus
    {
        Ready,
        Growing,
        Completed,
        Aborded
    }
}

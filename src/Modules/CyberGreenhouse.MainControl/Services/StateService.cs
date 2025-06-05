namespace CyberGreenhouse.MainControl.Services
{
    public class StateService
    {
        public MainControlStatus CurrentState { get; set; }

        public string? ErrorModule { get; set; }
        public string? Error { get; set; }

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

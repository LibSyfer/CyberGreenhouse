namespace CyberGreenhouse.MaturityMonitoring.MaturityMonitoringControlModule.Services
{
    public class StateService
    {
        public MaturityMonitoringStatus CurrentState { get; set; }

        public StateService()
        {
            CurrentState = MaturityMonitoringStatus.NotWork;
        }
    }

    public enum MaturityMonitoringStatus
    {
        NotWork,
        WaitTimeTrigger,
        WaitVisualTrigger
    }
}

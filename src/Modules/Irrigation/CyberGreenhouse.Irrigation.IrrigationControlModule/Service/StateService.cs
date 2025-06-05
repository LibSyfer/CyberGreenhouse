namespace CyberGreenhouse.Irrigation.IrrigationControlModule.Service
{
    public class StateService
    {
        public IrrigationStatus CurrentState { get; set; }
    }

    public enum IrrigationStatus
    {
        NotWork,
        PrepareFertilizer,
        Work
    }
}

namespace CyberGreenhouse.MessageBus.Contracts.Commands
{
    public class SetLightingLevelCommand
    {
        public double LightIntensity { get; set; }                  // Интенсивность света в люксах

        public int LightDuration { get; set; }                      //Продолжительность светового дня в часах
    }
}

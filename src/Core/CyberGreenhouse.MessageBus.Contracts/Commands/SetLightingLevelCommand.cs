using CyberGreenhouse.MessageBus.Abstractions;

namespace CyberGreenhouse.MessageBus.Contracts.Commands
{
    public class SetLightingLevelCommand : BusMessage
    {
        public double LightIntensity { get; set; }                  // Интенсивность света в люксах

        public int LightDuration { get; set; }                      //Продолжительность светового дня в часах
    }
}

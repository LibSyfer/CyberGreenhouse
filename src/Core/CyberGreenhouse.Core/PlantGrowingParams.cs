namespace CyberGreenhouse.Core
{
    public class PlantGrowingParams
    {
        public Guid Id { get; set; }

        public Guid TomatoId { get; set; }

        public double LightIntensity { get; set; }                  // Интенсивность света в люксах

        public int LightDuration { get; set; }                      //Продолжительность светового дня в часах

        public double AirTemperature { get; set; }                  // Температура воздуха

        public double WaterTemperature { get; set; }                // Температура воды полива

        public double HumidityLevel { get; set; }= 0;               // Влажность воздуха

        public double SoilHumidity { get; set; }                    // Влажность почвы

        public double FertilizerConcentrationPpm { get; set; }      // Концентрация удобрений

        public int MinGrowthSeconds { get; set; }                   // Минимальный период роста
    }
}

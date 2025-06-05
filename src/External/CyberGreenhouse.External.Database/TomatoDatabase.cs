using CyberGreenhouse.Core;

namespace CyberGreenhouse.External.Database
{
    public static class TomatoDatabase
    {
        public static TomatoEntity[] Tomatos =
        [
            new TomatoEntity { Id = Guid.Parse("715CD946-D92F-4B80-B1FF-1ED7F42387DA"), Name = "Мягкий" },
            new TomatoEntity { Id = Guid.Parse("BEBD76C1-5915-4B7B-844F-1FBCC53EF4F0"), Name = "Сочный" },
            new TomatoEntity { Id = Guid.Parse("5515DBBA-BD77-4B30-975D-C3D49D931D82"), Name = "Кислый" }
        ];

        public static PlantGrowingParams[] Params =
        [
            new PlantGrowingParams
            {
                Id = Guid.Parse("DF30DBEC-E088-4A62-A788-A023ED744029"),
                TomatoId = Tomatos[0].Id,
                LightIntensity = 35,        // 20 - 70
                LightDuration = 14,         // 10 - 20
                AirTemperature = 25,        // 20 - 40
                WaterTemperature = 23,      // 20 - 40
                HumidityLevel = 80,         // 20 - 80
                SoilHumidity = 65,          // 60 - 80
                FertilizerConcentrationPpm = 30,
                MinGrowthSeconds = 180
            },
            new PlantGrowingParams
            {
                Id = Guid.Parse("629A58D6-1C63-4B19-B15D-4E131CB48F4B"),
                TomatoId = Tomatos[0].Id,
                LightIntensity = 40,
                LightDuration = 12,
                AirTemperature = 38,
                WaterTemperature = 20,
                HumidityLevel = 60,
                SoilHumidity = 73,
                FertilizerConcentrationPpm = 55,
                MinGrowthSeconds = 120
            },
            new PlantGrowingParams
            {
                Id = Guid.Parse("0E8D8080-FA13-4448-B134-472C6101387C"),
                TomatoId = Tomatos[1].Id,
                LightIntensity = 67,
                LightDuration = 18,
                AirTemperature = 30,
                WaterTemperature = 38,
                HumidityLevel = 55,
                SoilHumidity = 64,
                FertilizerConcentrationPpm = 66,
                MinGrowthSeconds = 60
            }
        ];
    }
}

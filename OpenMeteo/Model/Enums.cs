using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMeteoMain.Model
{
    internal enum WeatherCode
    {
        ClearSky = 0,
        MainlyClear = 1,
        PartlyCloudy = 2,
        Overcast = 3,
        Fog = 45,
        DepositingRimeFog = 48,
        DrizzleLightIntensity = 51,
        DrizzleModerateIntensity = 53,
        DrizzleDenseIntensity = 55,
        FreezingDrizzleLightIntensity = 56,
        FreezingDrizzleDenseIntensity = 57,
        RainSlightIntensity = 61,
        RainModerateIntensity = 63,
        RainHeavyIntensity = 65,
        FreezingRainLightIntensity = 66,
        FreezingRainHeavyIntensity = 67,
        SnowFallSlightIntensity = 71,
        SnowFallModerateIntensity = 73,
        SnowFallHeavyIntensity = 75,
        SnowGrains = 77,
        RainShowersSlightIntensity = 80,
        RainShowersModerateIntensity = 81,
        RainShowersViolentIntensity = 82,
        SnowShowersSlightIntensity = 85,
        SnowShowersHeavyIntensity = 86,
        ThunderstormSlightOrModerate = 95,
        ThunderstormWithSlightHail = 96,
        ThunderstormWithHeavyHail = 99
    }
}

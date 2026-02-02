public static class WeatherCodeMapper
{
    public static WeatherType FromWeatherId(int id)
    {
        if (id >= 200 && id < 300)
            return WeatherType.Thunderstorm;

        if (id >= 300 && id < 600)
            return WeatherType.Rain;

        if (id >= 600 && id < 700)
            return WeatherType.Snow;

        if (id >= 700 && id < 800)
            return WeatherType.Atmosphere;

        if (id == 800)
            return WeatherType.Clear;

        return WeatherType.Clouds;
    }
}

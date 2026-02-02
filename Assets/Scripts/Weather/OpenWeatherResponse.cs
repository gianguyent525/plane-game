[System.Serializable]
public class OpenWeatherResponse
{
    public WeatherInfo[] weather;
}

[System.Serializable]
public class WeatherInfo
{
    public int id;
    public string main;
    public string description;
    public string icon;
}

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class WeatherApiClient : MonoBehaviour
{
    [SerializeField] private WeatherApiConfig config;

    string ApiKey => config.apiKey;

    public IEnumerator GetWeather(float lat, float lon, System.Action<WeatherData> callback)
    {
        string url =
            $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={ApiKey}";

        using var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            yield break;

        var response = JsonUtility.FromJson<OpenWeatherResponse>(req.downloadHandler.text);
        var w = response.weather[0];

        callback?.Invoke(new WeatherData
        {
            weatherType = WeatherCodeMapper.FromWeatherId(w.id),
            intensity = CalculateIntensity(w.id),
            description = w.description,
            iconCode = w.icon
        });
    }

    float CalculateIntensity(int id)
    {
        if (id == 500 || id == 600) return 0.3f;
        if (id == 501 || id == 601) return 0.6f;
        if (id >= 502 || id >= 602) return 1f;
        return 0.5f;
    }
}

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
        if (response?.weather == null || response.weather.Length == 0)
            yield break;

        var w = response.weather[0];

        Debug.Log($"Received weather: ID={w.id}, Desc={w.description}, Icon={w.icon}");
        int displayWeatherId = ResolveDisplayWeatherId(w.id);
        
        callback?.Invoke(new WeatherData
        {
            weatherType = WeatherCodeMapper.FromWeatherId(displayWeatherId),
            intensity = CalculateIntensity(displayWeatherId),
            description = w.description,
            iconCode = w.icon
        });
    }

    int ResolveDisplayWeatherId(int weatherId)
    {
        if (weatherId < 800)
            return weatherId;

    
        int randomStep = Random.Range(0, 7);
        Debug.Log($"Weather ID 800 (Clear) resolved to {weatherId - (randomStep * 100)}");
        return weatherId - (randomStep * 100);
    }

    float CalculateIntensity(int id)
    {
        int last = id % 100;

        if (last == 0) return 0.3f; // light intensity
        if (last == 1) return 0.6f; // medium intensity
        if (last >= 2 && last <= 99) return 1f; // heavy intensity

        return 0.5f;
    }
}

using UnityEngine;
public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    [Header("Mode")]
    public bool useManualWeather;
    public WeatherType manualWeather;

    public WeatherType CurrentWeather { get; private set; }

    [SerializeField] WeatherApiClient apiClient;
    [SerializeField] WeatherEffectController effectController;
    [SerializeField] GameplayWeatherModifier gameplayModifier;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (useManualWeather)
        {
            ApplyWeather(manualWeather);
        }
        else
        {
            StartCoroutine(FetchWeatherPeriodically());

        }
    }

    System.Collections.IEnumerator FetchWeatherPeriodically()
    {
        while (true)
        {
            float lat = Random.Range(-90f, 90f);      // vĩ độ
            float lon = Random.Range(-180f, 180f);    // kinh độ

            yield return StartCoroutine(apiClient.GetWeather(lat, lon, OnWeatherReceived));

            yield return new UnityEngine.WaitForSeconds(10f);
            Debug.Log("lat: " + lat + ", lon: " + lon);
        }
    }

    void OnWeatherReceived(WeatherData data)
    {
        ApplyWeather(data.weatherType, data.intensity);
    }

    public void ApplyWeather(WeatherType type, float intensity = 1f)
    {
        CurrentWeather = type;

        effectController.ApplyEffect(type, intensity);
        gameplayModifier.ApplyGameplayEffect(type, intensity);
    }
}

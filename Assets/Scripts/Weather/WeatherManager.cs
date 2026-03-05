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
            StartCoroutine(apiClient.GetWeather(10.8231f, 106.6297f, OnWeatherReceived));
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

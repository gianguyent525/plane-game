using UnityEngine;
public class GameplayWeatherModifier : MonoBehaviour
{
    public PlayerMove player;

    [Header("Movement multipliers at max weather intensity")]
    [Range(0.4f, 1.2f)] [SerializeField] float clearSpeedMultiplier = 1f;
    [Range(0.4f, 1.2f)] [SerializeField] float rainSpeedMultiplier = 0.88f;
    [Range(0.4f, 1.2f)] [SerializeField] float cloudsSpeedMultiplier = 0.96f;
    [Range(0.4f, 1.2f)] [SerializeField] float snowSpeedMultiplier = 0.78f;
    [Range(0.4f, 1.2f)] [SerializeField] float thunderstormSpeedMultiplier = 0.72f;
    [Range(0.4f, 1.2f)] [SerializeField] float atmosphereSpeedMultiplier = 0.84f;

    public void ApplyGameplayEffect(WeatherType type, float intensity)
    {
        if (player == null) return;

        // ensure intensity is between 0 and 1
        // min intensity = 0.3
        intensity = Mathf.Clamp01(intensity); 

        switch (type)
        {
            case WeatherType.Clear:
                SetSpeedByIntensity(clearSpeedMultiplier, intensity);
                break;

            case WeatherType.Rain:
                SetSpeedByIntensity(rainSpeedMultiplier, intensity);
                break;

            case WeatherType.Clouds:
                SetSpeedByIntensity(cloudsSpeedMultiplier, intensity);
                break;

            case WeatherType.Snow:
                SetSpeedByIntensity(snowSpeedMultiplier, intensity);
                break;
            case WeatherType.Thunderstorm:
                SetSpeedByIntensity(thunderstormSpeedMultiplier, intensity);
                break;
            case WeatherType.Atmosphere:
                SetSpeedByIntensity(atmosphereSpeedMultiplier, intensity);
                break;

            default:
                ResetGameplay();
                break;
        }
    }

    void SetSpeedByIntensity(float targetMultiplier, float intensity)
    {   
        // linearly interpolate between normal speed (1) and target multiplier based on intensity
        // Lerp(a, b, t) = a + (b - a) * t
        float finalMultiplier = Mathf.Lerp(1f, targetMultiplier, intensity);

        player.SetSpeedMultiplier(finalMultiplier);
    }

    void ResetGameplay()
    {
        // reset to normal when weather is unknown
        player?.SetSpeedMultiplier(1f);
    }
}

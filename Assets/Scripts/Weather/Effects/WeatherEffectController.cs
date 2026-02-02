using UnityEngine;

public class WeatherEffectController : MonoBehaviour
{
    [Header("Weather Prefabs")]
    public GameObject rainPrefab;
    public GameObject snowPrefab;
    public GameObject atmospherePrefab; 
    public GameObject stormPrefab;

    [Header("UI Elements")]
    public GameObject darkOverlay;

    GameObject currentEffect;

    public void ApplyEffect(WeatherType type, float intensity)
    {
        if (currentEffect) Destroy(currentEffect);

        // Control DarkOverlay visibility based on weather type
        if (darkOverlay != null)
        {
            darkOverlay.SetActive(type == WeatherType.Thunderstorm);
        }

        GameObject prefab = type switch
        {
            WeatherType.Rain => rainPrefab,
            WeatherType.Snow => snowPrefab,
            WeatherType.Atmosphere => atmospherePrefab,
            WeatherType.Thunderstorm => stormPrefab,
            _ => null
        };

        // if (!prefab) return;

        if (!prefab)
        {
            Debug.LogWarning($"No prefab assigned for weather type: {type}");
            return;
        }

        Debug.Log($"Instantiating prefab: {prefab.name}");

        currentEffect = Instantiate(prefab, transform);
        var ps = currentEffect.GetComponent<ParticleSystem>();
        if (ps == null)
            ps = currentEffect.GetComponentInChildren<ParticleSystem>();
        
        if (ps != null)
        {
            var emission = ps.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(100 * intensity);
            // Set simulation speed to 6 for Thunderstorm weather
            if (type == WeatherType.Thunderstorm)
            {
                var main = ps.main;
                main.simulationSpeed = 6f;
            }
        }
    }
}

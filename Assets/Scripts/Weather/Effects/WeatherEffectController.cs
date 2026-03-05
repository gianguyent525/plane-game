using UnityEngine;
using UnityEngine.VFX;

public class WeatherEffectController : MonoBehaviour
{
    [Header("Weather Prefabs")]
    public GameObject rainPrefab;
    public GameObject snowPrefab;
    public GameObject atmospherePrefab; 
    public GameObject stormPrefab;
    public GameObject cloudPrefab;

    [Header("UI Elements")]
    public GameObject darkOverlay;

    GameObject currentEffect;

    public void ApplyEffect(WeatherType type, float intensity)
    {
        if (currentEffect) Destroy(currentEffect);

        // Clear player's fog VFX reference when switching weather types
        ClearPlayerFogReference();

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
            WeatherType.Clouds => cloudPrefab,
            _ => null
        };

        // if (!prefab) return;

        if(type.ToString() == "Clear")
        {
            Debug.Log($"Instantiating prefab: Clear_effect");
            return;
        }

        if (!prefab)
        {
            Debug.LogWarning($"No prefab assigned for weather type: {type}");
            return;
        }

        Debug.Log($"Instantiating prefab: {prefab.name}");

        // Instantiate at world origin first
        currentEffect = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        
        // Parent to WeatherManager (this transform's parent) to keep scene organized
        if (transform.parent != null)
        {
            currentEffect.transform.SetParent(transform.parent, true); // true = keep world position
        }
        
        // Handle CloudEffect separately
        if (type == WeatherType.Clouds)
        {
            var cloudEffect = currentEffect.GetComponent<CloudEffect>();
            if (cloudEffect != null)
            {
                cloudEffect.SetIntensity(intensity);
            }
        }
        else
        {
            // Handle ParticleSystem effects (Rain, Snow, Atmosphere, Thunderstorm)
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

        // Connect Fog Effect to Player for interactive clearing
        if (type == WeatherType.Atmosphere)
        {
            var fogVFX = currentEffect.GetComponent<VisualEffect>();
            if (fogVFX != null)
            {
                var player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    var playerMove = player.GetComponent<PlayerMove>();
                    if (playerMove != null)
                    {
                        playerMove.vfxRenderer = fogVFX;
                    }
                }
            }
        }
    }

    void ClearPlayerFogReference()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var playerMove = player.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.vfxRenderer = null;
            }
        }
    }
}

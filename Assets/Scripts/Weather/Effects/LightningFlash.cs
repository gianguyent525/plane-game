using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningFlash : MonoBehaviour
{
    [Header("Lightning Settings")]
    [Tooltip("Tần suất lightning flash (giây)")]
    public float minFlashInterval = 2f;
    public float maxFlashInterval = 5f;
    
    [Tooltip("Thời gian flash sáng (giây)")]
    public float flashDuration = 0.1f;
    
    [Tooltip("Độ sáng khi flash")]
    public float flashIntensity = 3f;

    Light2D light2D;
    float nextFlashTime;

    public OverlayFlash overlayFlash;


    void Start()
    {
        light2D = GetComponent<Light2D>();
        
        if (light2D == null)
        {
            Debug.LogError("Lightning_Flash needs Light2D component!");
            enabled = false;
            return;
        }
        
        light2D.intensity = 0f;
        
        // Auto-find OverlayFlash if not assigned
        if (overlayFlash == null)
        {
            overlayFlash = FindFirstObjectByType<OverlayFlash>();
            if (overlayFlash == null)
            {
                Debug.LogWarning("OverlayFlash not found in scene!");
            }
        }
        
        ScheduleNextFlash();
    }

    void Update()
    {
        if (Time.time >= nextFlashTime)
        {
            StartCoroutine(Flash());
            ScheduleNextFlash();
        }
    }

    System.Collections.IEnumerator Flash()
    {
        light2D.intensity = flashIntensity;
        overlayFlash?.Flash();

        yield return new WaitForSeconds(flashDuration);

        light2D.intensity = 0f;
        
        if (Random.value > 0.5f)
        {
            yield return new WaitForSeconds(0.05f);
            light2D.intensity = flashIntensity * 0.7f;
            overlayFlash?.Flash();
            yield return new WaitForSeconds(flashDuration * 0.5f);
            light2D.intensity = 0f;
        }
    }

    void ScheduleNextFlash()
    {
        nextFlashTime = Time.time + Random.Range(minFlashInterval, maxFlashInterval);
    }
}

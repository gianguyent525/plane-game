using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverlayFlash : MonoBehaviour
{
    Image overlay;

    [Header("Dark Overlay Settings")]
    public float normalAlpha = 0.7f; // độ tối của overlay khi không flash

    [Header("Flash Settings")]
    public float flashDuration = 0.15f; // thời gian flash trắng sáng

    [Tooltip("Độ sáng trắng khi flash (0-1)")]
    public float whiteFlashIntensity = 0.7f; // độ sáng trắng khi flash

    [Tooltip("Thời gian fade từ trắng về tối")]
    public float fadeBackDuration = 0.3f; // thời gian fade từ trắng về tối

    Color normalColor;

    void Awake()
    {
        overlay = GetComponent<Image>();
        normalColor = overlay.color;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // Pha 1: Chuyển sang trắng sáng lóa
        overlay.color = new Color(1f, 1f, 1f, whiteFlashIntensity);
        yield return new WaitForSeconds(flashDuration);

        // Pha 2: Fade mượt từ trắng sáng về overlay tối
        Color darkColor = new Color(normalColor.r, normalColor.g, normalColor.b, normalAlpha);
        float elapsed = 0f;
        Color startColor = overlay.color;

        while (elapsed < fadeBackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeBackDuration;
            overlay.color = Color.Lerp(startColor, darkColor, t);
            yield return null;
        }

        overlay.color = darkColor;
    }
}


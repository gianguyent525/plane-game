using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverlayFlash : MonoBehaviour
{
    Image overlay;

    public float normalAlpha = 0.45f;
    public float flashAlpha = 0f;
    public float flashDuration = 0.1f;

    void Awake()
    {
        overlay = GetComponent<Image>();
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        SetAlpha(flashAlpha);
        yield return new WaitForSeconds(flashDuration);
        SetAlpha(normalAlpha);
    }

    void SetAlpha(float a)
    {
        Color c = overlay.color;
        c.a = a;
        overlay.color = c;
    }
}

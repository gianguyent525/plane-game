using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;   // Reference to the UI Slider
    public float fillSpeed = 0.2f;

    void Update()
    {
        if (slider.value < slider.maxValue)
        {
            slider.value += Time.deltaTime * fillSpeed;
        }
    }
}
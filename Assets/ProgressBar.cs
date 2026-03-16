using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;   // Reference to the UI Slider
    public float fillDuration = 180f;
    public GameObject boss; // Reference to the boss

    private bool bossSpawned = false;

    void Update()
    {
        if (slider.value < slider.maxValue)
        {
            slider.value += (slider.maxValue / fillDuration) * Time.deltaTime;
        }

        if (!bossSpawned && slider.value >= slider.maxValue)
        {
            slider.value = slider.maxValue; // tránh vượt quá
            boss.SetActive(true);
            bossSpawned = true;
            slider.gameObject.SetActive(false);
        }
    }
}
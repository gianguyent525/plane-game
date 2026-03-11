using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;   // Reference to the UI Slider
    public float fillSpeed = 0.2f;
    public GameObject boss; // Reference to the boss

    private bool bossSpawned = false;

    void Update()
    {
        if (slider.value < slider.maxValue)
        {
            slider.value += Time.deltaTime * fillSpeed;
        }

        // When progress bar is full, spawn boss
        if (!bossSpawned && slider.value >= slider.maxValue)
        {
            boss.SetActive(true);
            bossSpawned = true;
        }
    }
}
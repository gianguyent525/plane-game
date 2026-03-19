using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public float fillDuration = 180f;
    public GameObject boss;
    public WaveManager waveManager;

    private bool bossSpawned = false;

    // --- BỔ SUNG: Cờ kiểm tra xem game đã bắt đầu chưa ---
    private bool isRunning = false;

    // Hàm này sẽ được gọi từ nút Xuất Kích
    public void StartProgress()
    {
        isRunning = true;
    }

    void Update()
    {
        // THÊM DÒNG NÀY: Nếu game chưa bắt đầu, bỏ qua không chạy code ở dưới
        if (!isRunning) return;

        if (slider.value < slider.maxValue)
        {
            slider.value += (slider.maxValue / fillDuration) * Time.deltaTime;
        }

        if (!bossSpawned && slider.value >= slider.maxValue)
        {
            slider.value = slider.maxValue;

            boss.SetActive(true);
            bossSpawned = true;
            slider.gameObject.SetActive(false);

            if (waveManager != null)
            {
                waveManager.StopSpawning();
            }
        }
    }
}
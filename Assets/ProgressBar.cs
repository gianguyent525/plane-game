using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public float fillDuration = 180f;
    public GameObject boss;
    public WaveManager waveManager;

    // --- BỔ SUNG 1: Liên kết Canvas của Boss ---
    [Header("UI Liên kết")]
    public GameObject bossCanvas;

    private bool bossSpawned = false;
    private bool isRunning = false;

    // Hàm này sẽ được gọi từ nút Xuất Kích
    public void StartProgress()
    {
        isRunning = true;
    }

    void Start()
    {
        // Đảm bảo Boss Canvas tắt lúc mới vào game 
        if (bossCanvas != null)
        {
            bossCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Nếu game chưa bắt đầu, bỏ qua không chạy code ở dưới
        if (!isRunning) return;

        if (slider.value < slider.maxValue)
        {
            slider.value += (slider.maxValue / fillDuration) * Time.deltaTime;
        }

        if (!bossSpawned && slider.value >= slider.maxValue)
        {
            slider.value = slider.maxValue;

            // 1. Kích hoạt Boss
            boss.SetActive(true);
            bossSpawned = true;

            // 2. Tắt thanh tiến trình cũ đi
            slider.gameObject.SetActive(false);

            // 3. BẬT BOSS CANVAS LÊN
            if (bossCanvas != null)
            {
                bossCanvas.SetActive(true);
            }

            // 4. Ngừng sinh quái từ WaveManager
            if (waveManager != null)
            {
                waveManager.StopSpawning();
            }

            // 5. BỔ SUNG 2: Dọn sạch quái vật cũ còn sót lại trên màn hình
            ClearRemainingEnemies();
        }
    }

    // Hàm dọn dẹp sân chơi cho Boss
    private void ClearRemainingEnemies()
    {
        // Tìm tất cả các object đang có tag "Enemy" (Nhớ đảm bảo Boss KHÔNG dùng tag này, hoặc Boss chưa được gắn tag lúc này)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Tiêu diệt tàn dư (Bạn có thể thêm code sinh ra hiệu ứng nổ ở đây nếu muốn)
            Destroy(enemy);
        }

        Debug.Log("HỆ THỐNG: Đã dọn sạch " + enemies.Length + " lính lác cũ để đón Boss!");
    }
}
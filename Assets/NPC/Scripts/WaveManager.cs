using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public string groupName;
        public GameObject enemyPrefab;
        public int count;
        public float spawnRate;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> groups;
        public float timeToNextWave;
    }

    [Header("Cấu hình trận đấu")]
    public List<Wave> waves;

    [Header("UI Giao diện")]
    public GameObject difficultyPanel; // Kéo bảng UI chọn độ khó vào đây

    private float screenTopY;
    private float screenMinX, screenMaxX;
    private bool isGameStarted = false;

    void Start()
    {
        CalculateScreenBounds();

        // Bật bảng chọn độ khó lên lúc mới vào game (đề phòng bạn quên bật trong scene)
        if (difficultyPanel != null) difficultyPanel.SetActive(true);

        // LƯU Ý: Không gọi StartCoroutine(RunGameLoop) ở đây nữa, phải chờ người chơi bấm nút!
    }

    void CalculateScreenBounds()
    {
        Camera cam = Camera.main;
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));

        screenTopY = topRight.y + 1f;
        screenMinX = bottomLeft.x + 0.5f;
        screenMaxX = topRight.x - 0.5f;
    }

    // --- CÁC HÀM NÀY SẼ ĐƯỢC GẮN VÀO NÚT BẤM TRÊN MÀN HÌNH ---
    public void SelectEasy()
    {
        DifficultyManager.currentDifficulty = GameDifficulty.Easy;
        StartWaves();
    }

    public void SelectMedium()
    {
        DifficultyManager.currentDifficulty = GameDifficulty.Normal;
        StartWaves();
    }

    public void SelectHard()
    {
        DifficultyManager.currentDifficulty = GameDifficulty.Hard;
        StartWaves();
    }

    // --- BẮT ĐẦU CHẠY GAME ---
    private void StartWaves()
    {
        if (isGameStarted) return; // Tránh tình trạng bấm nút nhiều lần
        isGameStarted = true;

        // Ẩn bảng chọn độ khó đi
        if (difficultyPanel != null) difficultyPanel.SetActive(false);

        // Bắt đầu gọi quái ra
        StartCoroutine(RunGameLoop());
    }

    IEnumerator RunGameLoop()
    {
        // Cho người chơi 2 giây chuẩn bị sau khi bấm nút
        yield return new WaitForSeconds(2f);

        foreach (Wave wave in waves)
        {
            Debug.Log("Bắt đầu: " + wave.waveName);

            foreach (EnemyGroup group in wave.groups)
            {
                // ÁP DỤNG ĐỘ KHÓ: Tính lại số lượng và tốc độ spawn
                int actualCount = DifficultyManager.GetEnemyCount(group.count);
                float actualSpawnRate = group.spawnRate * DifficultyManager.GetFireRateMultiplier();

                for (int i = 0; i < actualCount; i++)
                {
                    SpawnEnemy(group.enemyPrefab);

                    // Thời gian chờ giữa các con quái
                    yield return new WaitForSeconds(actualSpawnRate);
                }
            }

            Debug.Log("Xong " + wave.waveName + ". Nghỉ " + wave.timeToNextWave + "s");
            yield return new WaitForSeconds(wave.timeToNextWave);
        }

        Debug.Log("ĐÃ HOÀN THÀNH TẤT CẢ WAVE! VICTORY!");
    }

    void SpawnEnemy(GameObject prefab)
    {
        if (prefab == null) return;
        float randomX = Random.Range(screenMinX, screenMaxX);
        Vector3 spawnPos = new Vector3(randomX, screenTopY, 0);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
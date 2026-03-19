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

        [Tooltip("Tick vào ô này nếu là quái sinh ra ở mép dưới màn hình")]
        public bool spawnAtBottom; // Thêm biến định hướng đẻ quái
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

    // Các biến tự động tính toán (Không hiện trên Inspector)
    private float screenTopY;
    private float screenBottomY;
    private float screenMinX, screenMaxX;
    private bool isGameStarted = false;

    void Start()
    {
        CalculateScreenBounds();
    }

    void CalculateScreenBounds()
    {
        Camera cam = Camera.main;
        // Tính toán tự động 4 góc của Camera
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));

        // Gán tọa độ điểm đẻ quái
        screenTopY = topRight.y + 1f;        // Mép trên (cộng thêm 1 chút để giấu quái)
        screenBottomY = bottomLeft.y - 1f;   // Mép dưới (trừ đi 1 chút để giấu quái)
        screenMinX = bottomLeft.x + 0.5f;    // Mép trái
        screenMaxX = topRight.x - 0.5f;      // Mép phải
    }

    public void StartWaves()
    {
        if (isGameStarted) return;
        isGameStarted = true;
        Debug.Log("WaveManager: Đã nhận lệnh, chuẩn bị thả quái sau 2 giây...");
        StartCoroutine(RunGameLoop());
    }

    IEnumerator RunGameLoop()
    {
        yield return new WaitForSeconds(2f);

        foreach (Wave wave in waves)
        {
            Debug.Log("Bắt đầu: " + wave.waveName);
            foreach (EnemyGroup group in wave.groups)
            {
                int actualCount = DifficultyManager.GetEnemyCount(group.count);
                float actualSpawnRate = group.spawnRate * DifficultyManager.GetFireRateMultiplier();

                for (int i = 0; i < actualCount; i++)
                {
                    SpawnEnemy(group.enemyPrefab, group.spawnAtBottom); // Truyền thêm lệnh spawnAtBottom
                    yield return new WaitForSeconds(actualSpawnRate);
                }
            }
            Debug.Log("Xong " + wave.waveName + ". Nghỉ " + wave.timeToNextWave + "s");
            yield return new WaitForSeconds(wave.timeToNextWave);
        }
        Debug.Log("ĐÃ HOÀN THÀNH TẤT CẢ WAVE! VICTORY!");
    }

    void SpawnEnemy(GameObject prefab, bool spawnAtBottom)
    {
        if (prefab == null) return;

        float randomX = Random.Range(screenMinX, screenMaxX);

        // ĐỊNH HƯỚNG: Nếu true thì lấy tọa độ mép dưới, false thì mép trên
        float spawnY = spawnAtBottom ? screenBottomY : screenTopY;

        Instantiate(prefab, new Vector3(randomX, spawnY, 0), Quaternion.identity);
    }

    public void StopSpawning()
    {
        StopAllCoroutines(); // Hủy ngay lập tức Coroutine RunGameLoop đang chạy
        Debug.Log("WaveManager: Đã ngừng sinh quái vì Boss xuất hiện!");
    }
}
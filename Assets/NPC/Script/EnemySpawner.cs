using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 3f;
    public float spawnRangeX = 8f; // The X-axis range to spawn enemies across the screen

    private float timer;
    private float currentSpawnRate;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentSpawnRate = spawnInterval;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // Choose a random enemy type
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        // Tự động tính toán chiều rộng thực tế của Camera
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Choose a random X position inside the screen width
        float randomX = Random.Range(bottomLeft.x + 0.5f, topRight.x - 0.5f);
        
        // CỐ TÌNH thả ngay TRONG màn hình (dưới mép trên 1 khoảng) để nhìn thấy ngay lập tức!
        float spawnY = topRight.y - 1.0f;

        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        
        // Tự động ép SortingOrder lên rất cao để không bị Background che khuất
        SpriteRenderer[] renderers = spawnedEnemy.GetComponentsInChildren<SpriteRenderer>();
        foreach(var sr in renderers)
        {
            sr.sortingOrder = 50;
        }

        Debug.Log("Đã thả 1 quái: " + enemyPrefab.name + " tại vị trí " + spawnPos);
    }

    // This method is called by GameplayWeatherModifier.cs
    public void SetSpawnRate(float newRateMultiplier)
    {
        currentSpawnRate = spawnInterval / newRateMultiplier;
    }
}

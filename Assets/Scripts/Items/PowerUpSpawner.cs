using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject powerUpPrefab;

    [Header("Spawn Settings")]
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 20f;

    private float timer;
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
        timer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnPowerUp();
            timer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnPowerUp()
    {
        if (powerUpPrefab == null || _cam == null) return;

        // Tính vị trí X ngẫu nhiên trong màn hình
        Vector3 bottomLeft = _cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = _cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float randomX = Random.Range(bottomLeft.x + 0.5f, topRight.x - 0.5f);
        float spawnY = topRight.y + 1f;

        Instantiate(powerUpPrefab, new Vector3(randomX, spawnY, 0f), Quaternion.identity);
    }
}

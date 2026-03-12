using UnityEngine;

public class HealthPowerUpSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject healthPowerUpPrefab;

    [Header("Spawn Settings")]
    public float minSpawnInterval = 15f;
    public float maxSpawnInterval = 30f;

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
            SpawnHealthPowerUp();
            timer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnHealthPowerUp()
    {
        if (healthPowerUpPrefab == null || _cam == null) return;

        Vector3 bottomLeft = _cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = _cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float randomX = Random.Range(bottomLeft.x + 0.5f, topRight.x - 0.5f);
        float spawnY = topRight.y + 1f;

        Instantiate(healthPowerUpPrefab, new Vector3(randomX, spawnY, 0f), Quaternion.identity);
    }
}

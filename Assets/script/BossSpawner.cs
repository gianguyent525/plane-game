using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public float spawnInterval = 30f;

    private GameObject currentBoss;
    private float timer;

    void Update()
    {
        // If boss exists, do nothing
        if (currentBoss != null)
            return;

        // Count time after boss is dead
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            currentBoss = Instantiate(
                bossPrefab,
                new Vector3(0, 3f, 0),
                Quaternion.identity
            );

            timer = 0f;
        }
    }
}
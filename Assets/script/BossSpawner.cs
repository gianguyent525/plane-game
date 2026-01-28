using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public float spawnTime = 10f;

    private float timer;
    private bool spawned;

    void Update()
    {
        if (spawned) return;

        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            Instantiate(bossPrefab, new Vector3(0, 3f, 0), Quaternion.identity);
            spawned = true;
        }
    }
}

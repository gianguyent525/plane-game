using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public float spawnInterval = 30f;

    private GameObject currentBoss;
    private float timer;

    void Start()
    {
        // Bí quyết: Gán ngay timer = spawnInterval để khi thanh Progress vừa gọi thằng này dậy,
        // nó sẽ sinh ra Trùm ngay lập tức ở frame đầu tiên! Sau khi Trùm chết thì nó mới đợi tiếp.
        timer = spawnInterval;
    }

    void Update()
    {
        // If boss exists, do nothing
        if (currentBoss != null)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            currentBoss = Instantiate(
                bossPrefab,
                new Vector3(0, 3f, 0),
                bossPrefab.transform.rotation   // ? USE PREFAB ROTATION
            );

            timer = 0f;
        }
    }
}
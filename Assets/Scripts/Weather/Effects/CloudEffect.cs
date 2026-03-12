using UnityEngine;
using System.Collections;

public class CloudEffect : MonoBehaviour
{
    [Header("Cloud Sprites")]
    public Sprite[] cloudSprites; 

    [Header("Spawn Settings")]
    [Range(0.5f, 10f)]
    public float minSpawnInterval = 1f; // Thời gian spawn tối thiểu
    [Range(0.5f, 10f)]
    public float maxSpawnInterval = 3f; // Thời gian spawn tối đa
    public float minX = -8f; // Vị trí X thấp nhất để spawn mây
    public float maxX = 8f; // Vị trí X cao nhất để spawn mây
    public float spawnY = 6f; // Vị trí Y spawn mây (phía trên màn hình)

    [Header("Cloud Movement")]
    [Range(0.5f, 5f)]
    public float moveSpeed = 1.5f; // Tốc độ di chuyển của mây
    public float destroyY = -6f; // Vị trí Y để destroy mây
    
    [Header("Rendering")]
    public int sortingOrder = 5; // Sorting order để hiển thị trước background

    [Header("Cloud Appearance")]
    [Range(0.1f, 200f)]
    public float minScale = 0.5f; // Kích thước nhỏ nhất của mây
    [Range(0.1f, 200f)]
    public float maxScale = 1.2f; // Kích thước lớn nhất của mây
    [Range(0f, 1f)]
    public float minAlpha = 0.3f; // Độ trong suốt tối thiểu
    [Range(0f, 1f)]
    public float maxAlpha = 0.7f; // Độ trong suốt tối đa

    private float intensity = 1f;
    private Coroutine spawnCoroutine;

    void Start()
    {
        if (cloudSprites == null || cloudSprites.Length == 0)
        {
            Debug.LogError("CloudEffect: Chưa gán cloud sprites! Hãy gán 3 sprite cloud_0, cloud_1, cloud_2");
            enabled = false;
            return;
        }

        StartSpawning();
    }

    public void SetIntensity(float value)
    {
        // thời gian spawn 
        intensity = Mathf.Clamp01(value);
        minSpawnInterval = Mathf.Lerp(2f, 0.5f, intensity);
        maxSpawnInterval = Mathf.Lerp(5f, 2f, intensity);
    }

    public void StartSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
        
        spawnCoroutine = StartCoroutine(SpawnClouds());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            SpawnCloud();
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    void SpawnCloud()
    {
        // Tạo GameObject cho mây
        GameObject cloudObj = new GameObject("Cloud");

        // Thêm SpriteRenderer và thiết lập sprite ngẫu nhiên
        SpriteRenderer sr = cloudObj.AddComponent<SpriteRenderer>();
        sr.sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
        sr.sortingOrder = sortingOrder; // Render trước background

        // Thiết lập màu và độ trong suốt
        Color cloudColor = sr.color;
        cloudColor.a = Random.Range(minAlpha, maxAlpha);
        sr.color = cloudColor;

        // Thiết lập vị trí ngẫu nhiên (world position)
        float randomX = Random.Range(minX, maxX);
        cloudObj.transform.position = new Vector3(randomX, spawnY, 0f);
        
        // Set parent sau khi đã set position để giữ world position
        cloudObj.transform.SetParent(transform, true);

        // Thiết lập kích thước ngẫu nhiên
        float randomScale = Random.Range(minScale, maxScale);
        cloudObj.transform.localScale = Vector3.one * randomScale;

        // Thêm component để di chuyển mây
        CloudMover mover = cloudObj.AddComponent<CloudMover>();
        mover.moveSpeed = moveSpeed * Random.Range(0.8f, 1.2f);  // random tốc độ di chuyển cho mỗi mây
        mover.destroyY = destroyY;
    }

    void OnDestroy()
    {
        StopSpawning();
    }
}

public class CloudMover : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float destroyY = -6f;

    void Update()
    {
        // Di chuyển mây từ trên xuống (world space)
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);

        // Destroy khi ra khỏi màn hình (world position)
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}

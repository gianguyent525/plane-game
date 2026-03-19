using UnityEngine;

public class EnemyBomber : MonoBehaviour
{
    [Header("Di chuyển")]
    public float speed = 5f;

    [Header("Thả Bom")]
    public GameObject bombPrefab;
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 1.5f;
    public float padding = 1.0f; // Khoảng cách lùi vào từ mép màn hình (để bom không rớt ngay sát mép)

    // Các biến này giờ sẽ tự tính, không cần hiện lên Inspector nữa
    private float minX, maxX, minY, maxY;
    private float timer;

    void Start()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);

        // --- TÍNH TOÁN GIỚI HẠN MÀN HÌNH TỰ ĐỘNG ---
        CalculateScreenBounds();
    }

    void CalculateScreenBounds()
    {
        Camera cam = Camera.main; // Lấy Camera chính đang chạy

        // Viewport: (0,0) là góc dưới trái, (1,1) là góc trên phải
        // Chuyển đổi sang tọa độ thế giới
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Gán giá trị + trừ hao (padding) để bom nằm gọn trong màn hình
        minX = bottomLeft.x + padding;
        maxX = topRight.x - padding;
        minY = bottomLeft.y + padding;
        maxY = topRight.y - padding;
    }

    void Update()
    {
        // 1. Bay thẳng lên trên
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);

        // 2. Logic thả bom
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnBomb();
            timer = Random.Range(minSpawnTime, maxSpawnTime);
        }

        // 3. Tự hủy khi bay ra khỏi màn hình
        // Dùng luôn giá trị maxY vừa tính để biết khi nào bay qua khỏi màn hình
        if (transform.position.y > maxY + 5f) // +5f để bay hẳn ra ngoài rồi mới xóa
        {
            Destroy(gameObject);
        }
    }

    void SpawnBomb()
    {
        if (bombPrefab == null) return;

        // Bây giờ minX, maxX đã chuẩn theo màn hình hiện tại
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(randomX, randomY, 0);

        Instantiate(bombPrefab, spawnPos, Quaternion.identity);
    }
}
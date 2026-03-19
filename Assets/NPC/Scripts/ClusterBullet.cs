using UnityEngine;

public class ClusterBullet : MonoBehaviour
{
    public float speed = 3f;
    public float explodeY = 0f; // Tọa độ Y đạn sẽ nổ (0 thường là giữa màn hình)

    public GameObject smallBulletPrefab; // Kéo Prefab Đạn Nhỏ vào đây
    public int fragmentCount = 8; // Số đạn túa ra

    void Update()
    {
        // Đạn lớn bay thẳng xuống theo trục thế giới
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        // Nổ khi bay đến giữa màn hình
        if (transform.position.y <= explodeY)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (smallBulletPrefab != null)
        {
            float angleStep = 360f / fragmentCount;
            for (int i = 0; i < fragmentCount; i++)
            {
                // Xoay từng viên đạn nhỏ theo vòng tròn
                Quaternion rotation = Quaternion.Euler(0, 0, i * angleStep);
                Instantiate(smallBulletPrefab, transform.position, rotation);
            }
        }
        Destroy(gameObject); // Hủy đạn lớn
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_health player = other.GetComponent<Player_health>();
            if (player != null) player.TakeDamage(2); // Trúng đạn lớn mất 2 máu
            Explode(); // Chạm Player cũng nổ luôn
        }
    }
}
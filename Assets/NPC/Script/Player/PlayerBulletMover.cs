using UnityEngine;

public class PlayerBulletMover : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Tự hủy sau 3s để đỡ lag
    }

    void Update()
    {
        // Bay LÊN trên (Vector3.up)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu đạn trúng kẻ địch
        if (other.CompareTag("Enemy"))
        {
            // Lấy script EnemyBase từ đối tượng va chạm
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Trừ máu kẻ địch
            }

            Destroy(gameObject); // Đạn biến mất sau khi trúng
        }
    }
}
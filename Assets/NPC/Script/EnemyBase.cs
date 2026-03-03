using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Chỉ số chung")]
    public float hp = 3;
    public float moveSpeed = 3f;
    public int scoreValue = 100;

    [Header("Vũ khí")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    protected float fireTimer;

    protected virtual void Start()
    {
        fireTimer = fireRate;
    }

    protected virtual void Update()
    {
        // Logic bắn súng cơ bản (đếm ngược)
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            Shoot();
            fireTimer = fireRate;
        }

        // Tự hủy nếu bay quá xa khỏi màn hình (Clean up)
        if (transform.position.y < -12f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Shoot()
    {
        Debug.Log("Piu Piu! Đang cố bắn đây!"); // Thêm dòng này để soi
        if (bulletPrefab && firePoint)
        {
            Instantiate(bulletPrefab, firePoint.position, bulletPrefab.transform.rotation);
        }
        else
        {
            Debug.Log("Lỗi: Thiếu BulletPrefab hoặc FirePoint rồi!"); // Báo lỗi nếu thiếu
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    // Hàm Die() để virtual để con Kamikaze có thể viết lại (Override) hành vi này
    protected virtual void Die()
    {
        // Mặc định là nổ và biến mất
        Debug.Log(gameObject.name + " đã bị tiêu diệt!");
        // Thêm hiệu ứng nổ (Explosion VFX) tại đây nếu có
        Destroy(gameObject);
    }

    // Xử lý va chạm với đạn của Player
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject); // Xóa đạn của player
        }
    }
}
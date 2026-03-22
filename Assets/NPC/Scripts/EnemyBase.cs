using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Vật phẩm rơi (Loot Drop)")]
    [Tooltip("Kéo các Prefab Boost (Máu, Súng...) vào đây")]
    public GameObject[] powerUpPrefabs;

    [Tooltip("Tỷ lệ rơi đồ khi chết (0% đến 100%)")]
    [Range(0f, 100f)]
    public float dropChance = 20f; // Mặc định là 20% rơi ra đồ

    [Header("Chỉ số chung")]
    public float hp = 3;
    public float moveSpeed = 3f;
    public int scoreValue = 100;
    protected bool isDead = false;
    private bool scoreAwarded = false;

    [Header("Vũ khí")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    protected float fireTimer;

    protected virtual void Start()
    {

        // --- ÁP DỤNG ĐỘ KHÓ ---
        moveSpeed *= DifficultyManager.GetSpeedMultiplier();
        fireRate *= DifficultyManager.GetFireRateMultiplier();
        // Bạn cũng có thể tăng máu ở đây: hp *= DifficultyManager.GetSpeedMultiplier();

        fireTimer = fireRate;
    }

    protected virtual void Update()
    {
        Move(); // Gọi hàm di chuyển riêng

        // Logic bắn súng
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            Shoot();
            fireTimer = fireRate;
        }

        CheckBounds(); // Gọi hàm dọn rác
    }

    // TÁCH RIÊNG: Để các con quái con dễ dàng thay đổi kiểu bay
    protected virtual void Move()
    {
        // Mặc định là bay thẳng xuống
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
    }

    // TÁCH RIÊNG: Dọn rác khi bay khỏi màn hình
    protected virtual void CheckBounds()
    {
        if (transform.position.y < -12f) Destroy(gameObject);
    }

    protected virtual void Shoot()
    {
        if (bulletPrefab && firePoint)
            Instantiate(bulletPrefab, firePoint.position, bulletPrefab.transform.rotation);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        hp -= damage;
        if (hp <= 0)
        {
            if (!scoreAwarded && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(scoreValue);
                scoreAwarded = true;
            }

            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        DropPowerUp();

        // Tạo hiệu ứng nổ, cộng điểm ở đây
        Destroy(gameObject);
    }

    // XỬ LÝ VA CHẠM CHUNG
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Đã XÓA phần check "PlayerBullet" vì script bullet.cs của Player đã tự lo việc đó rồi.

        // Chỉ giữ lại logic: Đâm thẳng vào Player thì nổ
        if (other.CompareTag("Player"))
        {
            Player_health player = other.GetComponent<Player_health>();
            if (player != null) player.TakeDamage(1);
            Die();
        }
    }

    // Hàm xử lý việc rơi vật phẩm
    protected void DropPowerUp()
    {
        // 1. Kiểm tra xem mình có cấu hình vật phẩm nào để rớt không
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0) return;

        // 2. Quay xổ số xem có trúng tỷ lệ rơi đồ không (Tung xúc xắc từ 0 đến 100)
        float randomRoll = Random.Range(0f, 100f);

        if (randomRoll <= dropChance)
        {
            // 3. Trúng giải! Bây giờ chọn ngẫu nhiên 1 trong các món đồ đang có
            int randomIndex = Random.Range(0, powerUpPrefabs.Length);
            GameObject itemToDrop = powerUpPrefabs[randomIndex];

            // 4. Sinh ra vật phẩm ngay tại vị trí máy bay địch vừa nổ
            if (itemToDrop != null)
            {
                Instantiate(itemToDrop, transform.position, Quaternion.identity);
                Debug.Log(gameObject.name + " vừa rớt ra một " + itemToDrop.name + "!");
            }
        }
    }
}
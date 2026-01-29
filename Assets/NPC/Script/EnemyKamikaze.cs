using UnityEngine;

public class EnemyKamikaze : EnemyBase
{
    [Header("Cấu hình cảm tử")]
    public float crashSpeed = 8f;
    public float rotationSpeed = 500f; // Tốc độ xoay vòng vòng khi rơi

    private bool isCrashing = false;
    private Vector3 crashDirection;

    protected override void Update()
    {
        if (isCrashing)
        {
            // Logic rơi tự do (Không gọi base.Update để ngừng bắn súng)
            transform.position += crashDirection * crashSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // Xóa nếu rơi khỏi màn hình
            if (transform.position.y < -12f || transform.position.x < -10f || transform.position.x > 10f)
            {
                Destroy(gameObject);
            }
            return;
        }

        // Nếu chưa chết thì hoạt động như bình thường (Bay thẳng xuống)
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        base.Update(); // Giữ tính năng bắn súng
    }

    // Ghi đè hàm Die của cha
    protected override void Die()
    {
        if (isCrashing) return; // Nếu đang rơi rồi thì thôi

        isCrashing = true;

        // 1. Vô hiệu hóa va chạm để không bị bắn tiếp (hoặc giữ nguyên nếu muốn player né)
        // GetComponent<Collider2D>().enabled = false; 

        // 2. Tìm vị trí Player hiện tại
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Tính hướng lao đầu vào chỗ Player đang đứng lúc đó
            crashDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            crashDirection = Vector3.down; // Không có player thì rơi thẳng
        }

        Debug.Log("KAMIKAZE!!! Lao xuống!");
        // Lưu ý: Không gọi Destroy(gameObject) ở đây
    }

    // Xử lý va chạm khi đang rơi trúng Player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCrashing && other.CompareTag("Player"))
        {
            // Gây sát thương lớn cho Player
            Debug.Log("Đâm trúng Player!");
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(5); // Kamikaze đâm trúng thì trừ nhiều máu (ví dụ 5)
            }
            Destroy(gameObject); // Nổ tan xác sau khi đâm
        }
        else
        {
            base.OnTriggerEnter2D(other); // Giữ logic bị đạn bắn khi chưa rơi
        }
    }
}
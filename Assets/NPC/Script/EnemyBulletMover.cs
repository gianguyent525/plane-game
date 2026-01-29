using UnityEngine;

public class EnemyBulletMover : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f; // Đạn tự hủy sau 5 giây để đỡ lag

    void Start()
    {
        // Hủy object sau 1 khoảng thời gian
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Bay xuống dưới (Vì là đạn của địch)
        // Lưu ý: Nếu đạn bay ngược, đổi Vector3.down thành Vector3.up
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    // Xử lý khi đạn trúng Player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Đạn trúng người chơi!");
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage(1); // Trừ 1 máu
            }

            Destroy(gameObject); // Đạn biến mất sau khi trúng
        }
    }
}
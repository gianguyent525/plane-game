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

            Player_health player = other.GetComponent<Player_health>();

            if (player != null)
            {
                player.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}
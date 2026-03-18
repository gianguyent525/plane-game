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
        // Đã sửa Vector3.right thành Vector3.down để bay thẳng xuống đúng nghĩa!
        transform.Translate(Vector3.down * speed * Time.deltaTime);
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
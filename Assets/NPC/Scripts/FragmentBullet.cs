using UnityEngine;

public class FragmentBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Đỡ rác map
    }

    void Update()
    {
        // QUAN TRỌNG: Không dùng Space.World ở đây
        // Bay theo hướng Vector3.down (hoặc up tùy thiết kế ảnh sprite của bạn)
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_health player = other.GetComponent<Player_health>();
            if (player != null) player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
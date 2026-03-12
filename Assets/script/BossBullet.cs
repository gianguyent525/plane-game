using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 6f;

    private Vector3 moveDirection;

    void Start()
    {
        // Use bullet's rotation to determine direction
        moveDirection = transform.rotation * Vector3.up;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Boss bullet hit player!");

            Player_health player = other.GetComponent<Player_health>();

            if (player != null)
            {
                player.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float lifeTime = 5f; // Destroy bullet after 3 seconds so game doesn't lag
    public int damage = 1;

    void Start()
    {
        // Destroy the bullet automatically after a few seconds
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move the bullet forward every frame
        transform.position += transform.up * speed * Time.deltaTime;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy hit!");
            }

            BossHealth boss = other.GetComponent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                Debug.Log("Boss hit!");
            }

            Destroy(gameObject);
        }
        else
        {
            // Also check if we hit the boss without a specific tag (just in case the tag is Untagged)
            BossHealth boss = other.GetComponent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                Debug.Log("Boss hit!");
                Destroy(gameObject);
            }
        }
    }
}
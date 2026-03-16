using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    public float fallSpeed = 2f; 
    public int healAmount = 1;   

    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;

        if (_cam != null)
        {
            float topEdge = _cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y;
            Vector3 pos = transform.position;
            pos.y = topEdge + 1f;
            transform.position = pos;
        }
    }

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (_cam != null)
        {
            float bottomEdge = _cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).y;
            if (transform.position.y < bottomEdge - 1f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player_health playerHealth = other.GetComponent<Player_health>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Debug.Log("Player đã hồi máu!");
            }

            Destroy(gameObject);
        }
    }
}
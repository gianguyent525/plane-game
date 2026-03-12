using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float fallSpeed = 2f;

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
            Debug.Log("Đã nhặt được vật phẩm!");
            Destroy(gameObject);
        }
    }
}
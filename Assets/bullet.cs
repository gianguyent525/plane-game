using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float lifeTime = 5f; // Destroy bullet after 3 seconds so game doesn't lag

    void Start()
    {
        // Destroy the bullet automatically after a few seconds
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move the bullet forward every frame
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}

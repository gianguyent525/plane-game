using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float fallSpeed = 2f;
    public AudioClip pickupClip;
    [Range(0f, 1f)] public float pickupVolume = 1f;

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
            mainweapon[] weapons = other.GetComponentsInChildren<mainweapon>();

            if (weapons.Length > 0)
            {
                foreach (mainweapon weapon in weapons)
                {
                    weapon.bulletCount = Mathf.Min(weapon.bulletCount + 1, weapon.maxBullet);
                    weapon.ResetDecayTimer();
                    Debug.Log("bulletCount = " + weapon.bulletCount);
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy mainweapon trên Player!");
            }

            if (pickupClip != null)
            {
                AudioSource.PlayClipAtPoint(pickupClip, transform.position, pickupVolume);
            }

            Destroy(gameObject);
        }
    }
}
using UnityEngine;
using System.Collections;

public class TimeBomb : MonoBehaviour
{
    [Header("Cấu hình Bom")]
    public float delayTime = 2.0f;
    public float explosionRadius = 2.0f;
    public int damage = 1;

    [Header("Hiệu ứng")]
    public GameObject explosionVFX;
    public SpriteRenderer warningSprite; // Kéo Sprite vòng tròn cảnh báo vào đây
    public float vfxDuration = 0.5f;

    [Header("Hiệu ứng Rơi")]
    public float startScale = 3.0f; // Kích thước lúc bắt đầu (To)
    public float endScale = 1.0f;   // Kích thước lúc nổ (Nhỏ/Chuẩn)

    void Start()
    {
        StartCoroutine(ExplodeProcess());
    }

    IEnumerator ExplodeProcess()
    {
        float timer = 0f;

        // Lưu lại kích thước ban đầu để tính toán
        Vector3 bigScale = new Vector3(startScale, startScale, 1f);
        Vector3 smallScale = new Vector3(endScale, endScale, 1f);

        // VÒNG LẶP THAY THẾ CHO WaitForSeconds
        // Chạy liên tục cho đến khi hết thời gian delayTime
        while (timer < delayTime)
        {
            timer += Time.deltaTime; // Cộng dồn thời gian trôi qua

            // Tính toán tiến độ (t) từ 0 đến 1 (0% -> 100%)
            float t = timer / delayTime;

            // Dùng Lerp để chuyển kích thước từ Big -> Small dựa theo tiến độ t
            if (warningSprite != null)
            {
                warningSprite.transform.localScale = Vector3.Lerp(bigScale, smallScale, t);
            }

            // Chờ đến khung hình tiếp theo rồi lặp lại
            yield return null;
        }

        // Đảm bảo khi hết giờ thì kích thước về đúng chuẩn (tránh sai số)
        if (warningSprite != null)
        {
            warningSprite.transform.localScale = smallScale;
        }

        // Sau khi thu nhỏ xong -> NỔ
        Explode();
    }

    void Explode()
    {
        // 1. Hiệu ứng nổ (VFX)
        if (explosionVFX != null)
        {
            GameObject vfxInstance = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfxInstance, vfxDuration);
        }

        // 2. Gây sát thương
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D obj in objectsHit)
        {
            if (obj.CompareTag("Player"))
            {
                PlayerController player = obj.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
        }

        // 3. Xóa bom
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
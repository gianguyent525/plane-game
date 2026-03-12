using UnityEngine;
using UnityEngine.InputSystem; // Cần thư viện này

public class PlayerController : MonoBehaviour
{
    [Header("Di chuyển")]
    public float moveSpeed = 10f;

    [Header("Chiến đấu")]
    public int currentHp = 10;
    public GameObject playerBulletPrefab; // Kéo Prefab đạn vào đây
    public Transform firePoint;           // Vị trí nòng súng

    void Update()
    {
        if (Keyboard.current == null) return;

        // --- 1. XỬ LÝ DI CHUYỂN (Code cũ) ---
        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY = -1f;

        Vector3 moveDir = new Vector3(moveX, moveY, 0).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        // --- 2. XỬ LÝ BẮN SÚNG (Mới thêm) ---
        // wasPressedThisFrame: Chỉ bắn 1 viên khi ấn xuống (Bán tự động)
        // Nếu muốn bắn liên thanh thì đổi thành .isPressed và thêm bộ đếm thời gian (Timer)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (playerBulletPrefab != null && firePoint != null)
        {
            // Tạo đạn tại vị trí firePoint, giữ nguyên góc xoay của Prefab
            Instantiate(playerBulletPrefab, firePoint.position, playerBulletPrefab.transform.rotation);
        }
    }

    // --- Code nhận sát thương (Giữ nguyên) ---
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("Máu Player còn: " + currentHp);
        if (currentHp <= 0) Destroy(gameObject);
    }
}
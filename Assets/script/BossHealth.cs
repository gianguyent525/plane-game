using UnityEngine;
using UnityEngine.UI; // Vẫn cần thư viện này để dùng UI Image

public class BossHealth : MonoBehaviour
{
    [Header("Chỉ số Boss")]
    public float baseHealth = 1500f;
    public float healthPerSecond = 5f;
    public float timeToSpawn = 180f;

    private float maxHealth;
    private float currentHealth;
    protected bool isDead = false;

    [Header("UI Liên kết")]
    [Tooltip("Kéo GameObject chứa nguyên cái khung thanh máu Boss vào đây")]
    public GameObject healthBarObject;

    // --- THAY ĐỔI Ở ĐÂY: Dùng Image thay vì Slider ---
    [Tooltip("Kéo Image BossHealthFill vào đây")]
    public Image healthFillImage;

    void Start()
    {
        // Tính toán lượng máu tối đa
        maxHealth = baseHealth + (healthPerSecond * timeToSpawn);
        currentHealth = maxHealth;

        // Đổ đầy thanh máu lúc mới sinh ra (fillAmount = 1 nghĩa là 100%)
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = 1f;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();
        Debug.Log("Boss HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        // --- THAY ĐỔI CÔNG THỨC Ở ĐÂY ---
        // fillAmount chỉ nhận giá trị từ 0.0 đến 1.0, nên ta lấy Máu Hiện Tại chia cho Máu Tối Đa
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }

    protected void Die()
    {
        isDead = true;
        Debug.Log("Boss bị tiêu diệt!");

        if (healthBarObject != null)
        {
            healthBarObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}
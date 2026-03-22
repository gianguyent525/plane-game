using UnityEngine;
using UnityEngine.UI; // Vẫn cần thư viện này để dùng UI Image
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Chỉ số Boss")]
    public float baseHealth = 1500f;
    public float healthPerSecond = 5f;
    public float timeToSpawn = 180f;
    public int scoreValue = 1000;

    [Header("Âm thanh chiến thắng")]
    public AudioClip bossDeathClip;
    [Range(0f, 1f)] public float bossDeathVolume = 1f;
    public AudioClip victoryClip;
    [Range(0f, 1f)] public float victoryVolume = 1f;
    [Tooltip("Nhạc nền")]
    public AudioSource bgmSource;

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
        // if (bgmSource == null)
        // {
        //     // Tìm đối tượng DifficultyUIController để lấy AudioSource nếu chưa được gán
        //     DifficultyUIController difficultyUI = FindObjectOfType<DifficultyUIController>();
        //     if (difficultyUI != null)
        //     {
        //         bgmSource = difficultyUI.bgmSource;
        //     }
        // }

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

        Player_health player = FindFirstObjectByType<Player_health>();
        if (player != null)
        {
            player.SetInvulnerable(true);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        if (healthBarObject != null)
        {
            healthBarObject.SetActive(false);
        }

        // hàm bất đồng bộ phát âm thanh và xử lý kết thúc sau khi boss chết
        StartCoroutine(HandleDeathAudioAndEnd());
    }

    private IEnumerator HandleDeathAudioAndEnd()
    {
        Vector3 playPosition = Camera.main != null ? Camera.main.transform.position : transform.position;

        if (bossDeathClip != null)
        {
            // Phát âm thanh chết của boss
            AudioSource.PlayClipAtPoint(bossDeathClip, playPosition, bossDeathVolume);
            yield return new WaitForSeconds(bossDeathClip.length);
        }

        if (victoryClip != null)
        {
            // Phát âm thanh chiến thắng sau khi âm thanh chết của boss kết thúc
            AudioSource.PlayClipAtPoint(victoryClip, playPosition, victoryVolume);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ShowGameOver();
        }

        Destroy(gameObject);
    }
}
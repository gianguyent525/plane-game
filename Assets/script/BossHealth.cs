using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float baseHealth = 100f;
    public float healthPerSecond = 5f;

    [Header("Death Settings")]
    public GameObject deathEffect;

    private float maxHealth;
    private float currentHealth;

    private GameObject bossHealthUI;
    private Image healthBarFill;

    private bool isDead = false;

    void Awake()
    {
        // Unity 6 safe method
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas not found in scene!");
            return;
        }

        // Must match name exactly
        Transform uiTransform = canvas.transform.Find("BossHealth");

        if (uiTransform == null)
        {
            Debug.LogError("BossHealth object not found under Canvas!");
            return;
        }

        bossHealthUI = uiTransform.gameObject;

        healthBarFill = bossHealthUI.GetComponentInChildren<Image>();

        if (healthBarFill == null)
        {
            Debug.LogError("No Image component found inside BossHealth!");
            return;
        }

        // Hide at start
        bossHealthUI.SetActive(false);
    }

    void Start()
    {
        Debug.Log("Boss Spawned");

        ScaleHealthWithTime();

        if (maxHealth <= 0)
            maxHealth = baseHealth;

        currentHealth = maxHealth;

        if (bossHealthUI != null)
            bossHealthUI.SetActive(true);

        UpdateHealthBar();
    }

    void ScaleHealthWithTime()
    {
        float timeSurvived = Time.timeSinceLevelLoad;
        maxHealth = baseHealth + (timeSurvived * healthPerSecond);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null && maxHealth > 0)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Boss Died");

        if (bossHealthUI != null)
            bossHealthUI.SetActive(false);

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
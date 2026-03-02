using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float baseHealth = 100f;
    public float healthPerSecond = 5f;

    public float maxHealth;
    public float currentHealth;

    [Header("UI")]
    public Image healthBarFill;

    [Header("Death Settings")]
    public GameObject deathEffect;

    private bool isDead = false;

    void Start()
    {
        ScaleHealthWithTime();
        currentHealth = maxHealth;
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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthBar();
            Die();
            return;
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Boss Died");

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}  
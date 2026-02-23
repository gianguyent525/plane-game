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

    void Start()
    {
        ScaleHealthWithTime();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        // test damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
        }
    }

    void ScaleHealthWithTime()
    {
        float timeSurvived = Time.timeSinceLevelLoad;
        maxHealth = baseHealth + (timeSurvived * healthPerSecond);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Player_health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Image[] hearts;
    public AudioClip gameOverClip;
    [Range(0f, 1f)] public float gameOverVolume = 1f;

    void Start()
    {
        //currentHealth = maxHealth;
        UpdateHearts();
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHearts();

        if (currentHealth <= 0)
        {
            Debug.Log("Player chết!");

            if (gameOverClip != null)
            {
                // Phát âm thanh game over tại vị trí của camera hoặc vị trí của player
                Vector3 playPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
                // Phát âm thanh với âm lượng đã thiết lập
                AudioSource.PlayClipAtPoint(gameOverClip, playPosition, gameOverVolume);
            }

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ShowGameOver();
            }

            Destroy(gameObject);
        }
    }
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHearts();
    }



    void UpdateHearts()
    {
        Debug.Log("Hearts length: " + hearts.Length);
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].color = Color.white;  // Full heart
            }
            else
            {
                hearts[i].color = Color.black;  // Lost heart
            }
        }
    }
}


 


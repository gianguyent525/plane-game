using UnityEngine;
using UnityEngine.UI;

public class Player_health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Image[] hearts;

    void Start()
    {
        currentHealth = maxHealth; // Đã mở khóa dòng này để bơm máu lúc bắt đầu
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
            
            // Submit the final score
            if (ScoreManager.Instance != null && HighScoreManager.Instance != null)
            {
                HighScoreManager.Instance.SubmitScore(ScoreManager.Instance.Score);
            }

            // Gọi bảng Game Over hiện lên
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
                hearts[i].color = Color.red;  // Full heart (Màu đỏ cho dễ nhìn)
            }
            else
            {
                // Lost heart (Màu xám mờ để vẫn nhìn thấy mờ mờ chứ ko bị tàng hình trong nền đen)
                hearts[i].color = new Color(0.5f, 0.5f, 0.5f, 0.5f); 
            }
        }
    }
}


 


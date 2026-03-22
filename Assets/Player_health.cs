using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player_health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    private bool isDead = false;
    private bool isInvulnerable = false;

    public Image[] hearts;
    public AudioClip playerDeathClip;
    [Range(0f, 1f)] public float playerDeathVolume = 1f;
    public AudioClip gameOverClip;
    [Range(0f, 1f)] public float gameOverVolume = 1f;

    void Start()
    {
        //currentHealth = maxHealth;
        UpdateHearts();
    }
    public void TakeDamage(int amount)
    {
        if (isDead || isInvulnerable) return;

        currentHealth -= amount;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHearts();

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Player chết!");
            // hàm bất đồng bộ phát âm thanh và xử lý game over sau khi chết
            StartCoroutine(HandleDeathAudioAndGameOver());
        }
    }
    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHearts();
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

    private IEnumerator HandleDeathAudioAndGameOver()
    {
        Vector3 playPosition = Camera.main != null ? Camera.main.transform.position : transform.position;

        if (playerDeathClip != null)
        {
            // Phát âm thanh chết của player
            AudioSource.PlayClipAtPoint(playerDeathClip, playPosition, playerDeathVolume);
            // Chờ cho âm thanh chết của player kết thúc trước khi tiếp tục
            yield return new WaitForSeconds(playerDeathClip.length);
        }

        if (gameOverClip != null)
        {
            // Phát âm thanh game over sau khi âm thanh chết của player kết thúc
            AudioSource.PlayClipAtPoint(gameOverClip, playPosition, gameOverVolume);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ShowGameOver();
        }

        Destroy(gameObject);
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


 


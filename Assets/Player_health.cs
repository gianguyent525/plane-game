using UnityEngine;
using UnityEngine.UI;

public class Player_health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public Image[] hearts;

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
            Destroy(gameObject);
        }
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


 


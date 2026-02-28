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
using UnityEngine;

public class mainweapon : MonoBehaviour
{
    float timeToActtack = 1f;
    float timer;
    [SerializeField] GameObject bullet;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Attack();
        }
    }
    private void Attack()
    {
        // This creates the bullet at the weapon's current position and rotation
        Instantiate(bullet, transform.position, transform.rotation);

        timer = timeToActtack;
    }
}

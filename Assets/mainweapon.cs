using UnityEngine;

public class mainweapon : MonoBehaviour
{

    public float timeToActtack = 0.5f; 
    float timer;
    [SerializeField] GameObject bullet;

    [SerializeField] Transform[] firePoints;

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

        foreach (Transform point in firePoints)
        {
            if (point != null) 
            {
                Instantiate(bullet, point.position, point.rotation);
            }
        }

        timer = timeToActtack;
    }
}
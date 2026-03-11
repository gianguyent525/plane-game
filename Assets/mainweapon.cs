using UnityEngine;

public class mainweapon : MonoBehaviour
{
    public float timeToActtack = 0.5f;
    float timer;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform[] firePoints;

    public int bulletCount = 1;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        float spreadAngle = GetSpreadAngle();

        float startAngle = -spreadAngle / 2f;
        float angleStep = bulletCount > 1 ? spreadAngle / (bulletCount - 1) : 0;

        for (int i = 0; i < bulletCount; i++)
        {
            Transform firePoint = firePoints[i % firePoints.Length];

            float angle = startAngle + angleStep * i;

            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);

            Instantiate(bullet, firePoint.position, rotation);
        }

        timer = timeToActtack;
    }

    float GetSpreadAngle()
    {
        switch (bulletCount)
        {
            case 1: return 0f;
            case 2: return 30f;
            case 3: return 50f;
            case 4: return 70f;
            case 5: return 90f;
            default: return 100f;
        }
    }
}
using UnityEngine;

public class BossShoot : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Base Fire Rate Settings")]
    public float startFireRate = 1.2f;
    public float minFireRate = 0.25f;
    public float fireRateDecreasePerSecond = 0.005f;

    private float currentFireRate;
    private float fireTimer;

    [Header("Spawn Difficulty Scaling")]
    public float spawnTime;
    public float difficultyMultiplier = 0.01f;

    [Header("Shooting Pattern")]
    public ShootPattern currentPattern;

    [Header("Spread Settings")]
    public int spreadBulletCount = 5;
    public float spreadAngle = 60f;

    [Header("Side Barrage Settings")]
    public float sideAngle = 25f;
    private bool shootLeftNext = true;

    [Header("Cross Burst Settings")]
    public int burstDirections = 8;

    void Start()
    {
        // ?? RANDOM PATTERN ON SPAWN
        currentPattern = (ShootPattern)Random.Range(0, System.Enum.GetValues(typeof(ShootPattern)).Length);

        Debug.Log("Boss Chosen Pattern: " + currentPattern);

        float difficulty = spawnTime * difficultyMultiplier;

        currentFireRate = startFireRate - difficulty;
        currentFireRate = Mathf.Clamp(currentFireRate, minFireRate, startFireRate);
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= currentFireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        switch (currentPattern)
        {
            case ShootPattern.Straight:
                ShootStraight();
                break;

            case ShootPattern.Spread:
                ShootSpread();
                break;

            case ShootPattern.SideBarrage:
                ShootSideBarrage();
                break;

            case ShootPattern.CrossBurst:
                ShootCrossBurst();
                break;
        }
    }

    // ================= PATTERNS =================

    void ShootStraight()
    {
        Vector3 direction = Vector3.down;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, direction);
        Instantiate(bulletPrefab, firePoint.position, rot);
    }

    void ShootSpread()
    {
        for (int i = 0; i < spreadBulletCount; i++)
        {
            float angleOffset = -spreadAngle / 2f +
                                spreadAngle * i / (spreadBulletCount - 1);

            Vector3 direction =
                Quaternion.Euler(0, 0, angleOffset) * Vector3.down;

            Quaternion rot =
                Quaternion.LookRotation(Vector3.forward, direction);

            Instantiate(bulletPrefab, firePoint.position, rot);
        }
    }

    void ShootSideBarrage()
    {
        ShootStraight();

        Vector3 baseDirection = Vector3.down;

        float angle = shootLeftNext ? sideAngle : -sideAngle;

        Vector3 sideDirection =
            Quaternion.Euler(0, 0, angle) * baseDirection;

        Quaternion rot =
            Quaternion.LookRotation(Vector3.forward, sideDirection);

        Instantiate(bulletPrefab, firePoint.position, rot);

        shootLeftNext = !shootLeftNext;
    }

    void ShootCrossBurst()
    {
        for (int i = 0; i < burstDirections; i++)
        {
            float angle = i * (360f / burstDirections);

            Vector3 direction =
                Quaternion.Euler(0, 0, angle) * Vector3.down;

            Quaternion rot =
                Quaternion.LookRotation(Vector3.forward, direction);

            Instantiate(bulletPrefab, firePoint.position, rot);
        }
    }
}

public enum ShootPattern
{
    Straight,
    Spread,
    SideBarrage,
    CrossBurst
}
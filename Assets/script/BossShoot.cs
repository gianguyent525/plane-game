using UnityEngine;

public class BossShoot : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Base Fire Rate Settings")]
    public float startFireRate = 1.2f;   // Base fire rate
    public float minFireRate = 0.25f;    // Fastest allowed
    public float fireRateDecreasePerSecond = 0.005f;

    private float currentFireRate;
    private float fireTimer;

    [Header("Spawn Difficulty Scaling")]
    public float spawnTime;              // Set by spawner
    public float difficultyMultiplier = 0.01f;

    [Header("Shooting Pattern")]
    public ShootPattern currentPattern;

    [Header("Spread Settings")]
    public int spreadBulletCount = 5;
    public float spreadAngle = 60f;

    [Header("V-Shape Settings")]
    public int vBulletCount = 4;
    public float vAngleStep = 12f;

    [Header("Cross Burst Settings")]
    public int burstDirections = 8;

    void Start()
    {
        // Scale fire rate based on when boss spawned
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

            case ShootPattern.ShootVShape:
                ShootVShape();
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

        Quaternion rot =
            Quaternion.LookRotation(Vector3.forward, direction);

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

    void ShootVShape()
    {
        Vector3 baseDirection = Vector3.down;

        for (int i = 1; i <= vBulletCount; i++)
        {
            float angleOffset = i * vAngleStep;

            // Left side
            Vector3 leftDir =
                Quaternion.Euler(0, 0, angleOffset) * baseDirection;

            Quaternion leftRot =
                Quaternion.LookRotation(Vector3.forward, leftDir);

            Instantiate(bulletPrefab, firePoint.position, leftRot);

            // Right side
            Vector3 rightDir =
                Quaternion.Euler(0, 0, -angleOffset) * baseDirection;

            Quaternion rightRot =
                Quaternion.LookRotation(Vector3.forward, rightDir);

            Instantiate(bulletPrefab, firePoint.position, rightRot);
        }
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
    ShootVShape,
    CrossBurst
}
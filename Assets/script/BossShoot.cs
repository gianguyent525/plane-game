using UnityEngine;

public class BossShoot : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Fire Rate")]
    public float startFireRate = 1.2f;
    public float minFireRate = 0.3f;
    public float fireRateDecreasePerSecond = 0.01f;

    private float currentFireRate;
    private float fireTimer;

    [Header("Shooting Pattern")]
    public ShootPattern currentPattern;

    [Header("Spread Settings")]
    public int spreadBulletCount = 5;
    public float spreadAngle = 60f;

    [Header("V-Shape Settings")]
    public int vBulletCount = 4;
    public float vAngleStep = 12f;

    [Header("Wave Settings")]
    public float waveAmplitude = 45f;
    public float waveSpeed = 2f;
    private float waveTimer;

    void Start()
    {
        currentFireRate = startFireRate;
    }

    void Update()
    {
        currentFireRate -= fireRateDecreasePerSecond * Time.deltaTime;
        currentFireRate = Mathf.Max(minFireRate, currentFireRate);

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

            case ShootPattern.Wave:
                ShootWave();
                break;
        }
    }

    // ================= PATTERNS =================

    void ShootStraight()
    {
        // Directly aim downward in world space
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

            Vector3 baseDirection = Vector3.down;
            Vector3 rotatedDirection =
                Quaternion.Euler(0, 0, angleOffset) * baseDirection;

            Quaternion rot =
                Quaternion.LookRotation(Vector3.forward, rotatedDirection);

            Instantiate(bulletPrefab, firePoint.position, rot);
        }
    }

    void ShootVShape()
    {
        Vector3 baseDirection = Vector3.down;

        for (int i = 1; i <= vBulletCount; i++)
        {
            float angleOffset = i * vAngleStep;

            // LEFT ARM
            Vector3 leftDir =
                Quaternion.Euler(0, 0, angleOffset) * baseDirection;

            Quaternion leftRot =
                Quaternion.LookRotation(Vector3.forward, leftDir);

            Instantiate(bulletPrefab, firePoint.position, leftRot);

            // RIGHT ARM
            Vector3 rightDir =
                Quaternion.Euler(0, 0, -angleOffset) * baseDirection;

            Quaternion rightRot =
                Quaternion.LookRotation(Vector3.forward, rightDir);

            Instantiate(bulletPrefab, firePoint.position, rightRot);
        }

    }

    void ShootWave()
    {
        waveTimer += Time.deltaTime * waveSpeed;
        float waveOffset = Mathf.Sin(waveTimer) * waveAmplitude;

        Vector3 baseDirection = Vector3.down;
        Vector3 rotatedDirection =
            Quaternion.Euler(0, 0, waveOffset) * baseDirection;

        Quaternion rot =
            Quaternion.LookRotation(Vector3.forward, rotatedDirection);

        Instantiate(bulletPrefab, firePoint.position, rot);
    }
}

public enum ShootPattern
{
    Straight,
    Spread,
    ShootVShape,
    Wave
}
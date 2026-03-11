using UnityEngine;

public class EnemyPatrol : EnemyBase
{
    [Header("Cấu hình bay lượn")]
    public float moveDownDistance = 3f;
    public float horizontalTime = 2f;
    public bool moveRightFirst = true;

    private int state = 0;
    private float startY;
    private float horizontalTimer;

    protected override void Start()
    {
        base.Start();
        startY = transform.position.y;
    }

    // GHI ĐÈ hàm Move của Base, không cần đụng tới Update
    protected override void Move()
    {
        if (state == 0)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
            if (transform.position.y <= startY - moveDownDistance)
            {
                state = 1;
                horizontalTimer = horizontalTime;
            }
        }
        else if (state == 1)
        {
            Vector3 direction = moveRightFirst ? Vector3.right : Vector3.left;
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            horizontalTimer -= Time.deltaTime;
            if (horizontalTimer <= 0) state = 2;
        }
        else if (state == 2)
        {
            transform.Translate(Vector3.down * moveSpeed * 1.5f * Time.deltaTime, Space.World);
        }
    }
}
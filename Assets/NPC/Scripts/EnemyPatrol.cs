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

    // Lưu giới hạn màn hình
    private float screenMinX, screenMaxX;

    protected override void Start()
    {
        base.Start();
        startY = transform.position.y;

        // Tự động tính mép Camera
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float padding = 0.5f; // Trừ hao nửa thân máy bay
        screenMinX = bottomLeft.x + padding;
        screenMaxX = topRight.x - padding;
    }

    protected override void Move()
    {
        if (state == 0) // Giai đoạn 1: Bay xuống
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
            if (transform.position.y <= startY - moveDownDistance)
            {
                state = 1;
                horizontalTimer = horizontalTime;
            }
        }
        else if (state == 1) // Giai đoạn 2: Bay ngang
        {
            Vector3 direction = moveRightFirst ? Vector3.right : Vector3.left;
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

            // LOGIC MỚI: Chạm mép Trái HOẶC Phải của Camera -> Chuyển sang bay thoát xuống dưới luôn
            if (transform.position.x <= screenMinX || transform.position.x >= screenMaxX)
            {
                state = 2;
            }

            // Hoặc nếu hết giờ bay ngang cũng cắm đầu xuống
            horizontalTimer -= Time.deltaTime;
            if (horizontalTimer <= 0) state = 2;
        }
        else if (state == 2) // Giai đoạn 3: Rút lui xuống đáy màn hình
        {
            transform.Translate(Vector3.down * moveSpeed * 1.5f * Time.deltaTime, Space.World);
        }
    }
}
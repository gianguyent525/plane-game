using UnityEngine;

public class EnemyPatrol : EnemyBase // Kế thừa từ EnemyBase
{
    [Header("Cấu hình bay lượn")]
    public float moveDownDistance = 3f; // Bay xuống bao xa thì rẽ
    public float horizontalTime = 2f;   // Bay ngang trong bao lâu
    public bool moveRightFirst = true;  // Rẽ phải hay trái

    private int state = 0; // 0: Xuống, 1: Ngang, 2: Thoát
    private float startY;
    private float horizontalTimer;

    protected override void Start()
    {
        base.Start(); // Gọi hàm Start của cha
        startY = transform.position.y;
    }

    protected override void Update()
    {
        base.Update(); // Gọi hàm Update của cha để giữ tính năng bắn súng
        MoveLogic();
    }

    void MoveLogic()
    {
        if (state == 0) // Giai đoạn 1: Bay xuống
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            // Nếu đã bay xuống đủ sâu -> Chuyển sang bay ngang
            if (transform.position.y <= startY - moveDownDistance)
            {
                state = 1;
                horizontalTimer = horizontalTime;
            }
        }
        else if (state == 1) // Giai đoạn 2: Bay ngang
        {
            Vector3 direction = moveRightFirst ? Vector3.right : Vector3.left;
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            horizontalTimer -= Time.deltaTime;
            if (horizontalTimer <= 0)
            {
                state = 2; // Hết giờ bay ngang -> Chuyển sang thoát
            }
        }
        else if (state == 2) // Giai đoạn 3: Bay xuống thoát màn hình
        {
            transform.Translate(Vector3.down * moveSpeed * 1.5f * Time.deltaTime); // Bay nhanh hơn để thoát
        }
    }
}
using UnityEngine;

public class EnemyInterceptor : EnemyBase
{
    [Header("Cấu hình Đánh Chặn")]
    public float dropSpeed = 4f;   // Tốc độ hạ cánh
    public float chaseSpeed = 5f;  // Tốc độ trượt ngang rình rập
    public float dashSpeed = 15f;  // Tốc độ lao xuống (Cực nhanh)

    public float stopY = 3f;       // Tọa độ Y sẽ dừng lại để rình
    public float aimTime = 1.5f;   // Thời gian rình trước khi lao

    private Transform player;
    private int state = 0; // 0: Đang hạ cánh, 1: Đang rình, 2: Đang lao xuống

    protected override void Start()
    {
        base.Start(); // Giữ nguyên hàm Start của cha

        // Tìm Player để chuẩn bị rình
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    // GHI ĐÈ hàm Move của EnemyBase
    protected override void Move()
    {
        if (state == 0) // Giai đoạn 0: Bay vào màn hình
        {
            transform.Translate(Vector3.down * dropSpeed * Time.deltaTime, Space.World);
            if (transform.position.y <= stopY)
            {
                state = 1; // Đạt độ cao thì chuyển sang rình
            }
        }
        else if (state == 1) // Giai đoạn 1: Trượt ngang theo Player
        {
            aimTime -= Time.deltaTime;

            if (player != null)
            {
                // Chỉ bám theo trục X của Player
                Vector3 targetPos = new Vector3(player.position.x, transform.position.y, 0);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
            }

            if (aimTime <= 0) state = 2; // Hết giờ thì lao
        }
        else if (state == 2) // Giai đoạn 2: Tử thần giáng lâm
        {
            transform.Translate(Vector3.down * dashSpeed * Time.deltaTime, Space.World);
        }
    }

    // GHI ĐÈ hàm Shoot: Con này KHÔNG bắn đạn, nên ta để trống hàm này
    protected override void Shoot()
    {
        // Không làm gì cả
    }
}
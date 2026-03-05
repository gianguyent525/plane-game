using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 move;

    public float speed = 5f;

    public VisualEffect vfxRenderer;

    [Header("Screen Bounds")]
    public float paddingLeft = 0.2f;
    public float paddingRight = 0.2f;
    public float paddingTop = 0f;      
    public float paddingBottom = 0f;

    private Camera mainCamera;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private Vector2 playerHalfSize;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        CalculatePlayerSize();
        CalculateScreenBounds();
    }

    void CalculatePlayerSize()
    {
        // Tính kích thước player 
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            playerHalfSize = spriteRenderer.bounds.extents;
        }
        else
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                playerHalfSize = col.bounds.extents;
            }
            else
            {
                playerHalfSize = new Vector2(0.5f, 0.5f); // Default size
            }
        }
    }

    void CalculateScreenBounds()
    {
        if (mainCamera == null) return;

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Tính padding riêng cho từng phía
        float leftBound = -camWidth + playerHalfSize.x + paddingLeft;
        float rightBound = camWidth - playerHalfSize.x - paddingRight;
        float bottomBound = -camHeight + playerHalfSize.y + paddingBottom;
        float topBound = camHeight - playerHalfSize.y - paddingTop;

        minBounds = new Vector2(leftBound, bottomBound);
        maxBounds = new Vector2(rightBound, topBound);
    }

    void Update()
    {
        move = Keyboard.current != null
            ? new Vector2(
                (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0),
                (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0)
              )
            : Vector2.zero;


        if (vfxRenderer != null)
        {
            vfxRenderer.SetVector3("ColliderPos", transform.position);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = move * speed;
    }

    void LateUpdate()
    {
        // Giới hạn vị trí player trong màn hình
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        transform.position = pos;
    }
}

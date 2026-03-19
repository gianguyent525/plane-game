using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HDRButtonGlow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Thành phần")]
    public Image glowRingImage;

    [Header("Màu sắc Phát sáng")]
    public Color glowColor = new Color(0f, 1f, 0.5f);

    [Header("Cường độ HDR")]
    public float idleIntensity = 0f;       // Tối thui khi để yên
    public float hoverIntensity = 1.2f;    // Sáng nhẹ khi lia chuột
    public float pressIntensity = 4.0f;    // Sáng rực rỡ khi ấn xuống
    public float selectedIntensity = 2.5f; // SÁNG DUY TRÌ KHI ĐƯỢC CHỌN (MỚI THÊM)

    [Header("Tốc độ chuyển đổi")]
    public float fadeSpeed = 15f;

    private float currentIntensity;
    private float targetIntensity;

    // Biến lưu trạng thái xem nút này có đang được chọn hay không
    private bool isSelected = false;

    void Start()
    {
        targetIntensity = idleIntensity;
        currentIntensity = idleIntensity;
    }

    void Update()
    {
        if (glowRingImage == null) return;

        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * fadeSpeed);

        Color hdrColor = new Color(
            glowColor.r * currentIntensity,
            glowColor.g * currentIntensity,
            glowColor.b * currentIntensity,
            1f
        );

        glowRingImage.color = hdrColor;
    }

    // HÀM MỚI: Bảng điều khiển sẽ gọi hàm này để bật/tắt trạng thái giữ sáng
    public void SetSelected(bool state)
    {
        isSelected = state;
        if (isSelected)
            targetIntensity = selectedIntensity; // Giữ sáng
        else
            targetIntensity = idleIntensity;     // Tắt đi
    }

    // Các sự kiện chuột giờ sẽ phải "hỏi ý kiến" biến isSelected
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected) targetIntensity = hoverIntensity;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) targetIntensity = idleIntensity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetIntensity = pressIntensity; // Luôn chớp sáng mạnh khi click
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isSelected) targetIntensity = selectedIntensity; // Nếu đang được chọn thì lùi về mức sáng duy trì
        else targetIntensity = hoverIntensity;
    }
}
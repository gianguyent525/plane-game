using UnityEngine;

namespace Game.Weather.Effects
{
    public class RainEffect : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Renderer rainRenderer; // Kéo MeshRenderer của Quad vào đây
        [SerializeField] private float fallSpeedY = 1.0f; // Tốc độ rơi dọc
        [SerializeField] private float windSpeedX = 0.1f; // Tốc độ gió ngang (tùy chọn)

        private Material rainMat;
        private Vector2 currentOffset;

        private void Awake()
        {
            // Tự động lấy Renderer nếu chưa gán
            if (rainRenderer == null)
                rainRenderer = GetComponent<Renderer>();

            // Lấy bản copy của material để không ảnh hưởng file gốc
            if (rainRenderer != null)
                rainMat = rainRenderer.material;
        }

        private void Update()
        {
            if (rainMat == null) return;

            // Tính toán độ dời của texture theo thời gian
            float offsetY = Time.time * fallSpeedY;
            float offsetX = Time.time * windSpeedX;

            // Cập nhật vị trí texture (tạo hiệu ứng cuộn)
            // Dùng dấu trừ (-) để mưa rơi từ trên xuống
            currentOffset = new Vector2(offsetX, -offsetY);

            rainMat.mainTextureOffset = currentOffset;
        }

        // Hàm gọi từ WeatherManager để đổi độ mạnh của mưa (đổi texture)
        public void SetIntensity(Texture newTexture, float speed)
        {
            if (rainMat != null && newTexture != null)
            {
                rainMat.mainTexture = newTexture;
            }
            fallSpeedY = speed;
        }
    }
}
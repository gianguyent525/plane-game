using UnityEngine;

namespace Game.Environment
{
    public class BackgroundScroller : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float scrollSpeed = 0.5f; // Tốc độ cuộn

        private Renderer bgRenderer;
        private Material bgMaterial;
        private Vector2 currentOffset;

        private void Awake()
        {
            bgRenderer = GetComponent<Renderer>();
            if (bgRenderer != null)
            {
                bgMaterial = bgRenderer.material;
            }
        }

        private void Update()
        {
            if (bgMaterial == null) return;

            float offsetY = Time.time * scrollSpeed;

            currentOffset = new Vector2(0, offsetY % 1);

            bgMaterial.mainTextureOffset = currentOffset;
        }
    }
}
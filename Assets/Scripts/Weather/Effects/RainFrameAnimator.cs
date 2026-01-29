using UnityEngine;

namespace Game.Weather.Effects
{
    public class RainFrameAnimator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Animation Frames")]
        [SerializeField] private Sprite[] rainFrames; 
        [SerializeField] private float frameRate = 12f; // Tốc độ nháy hình (FPS)

        private int currentFrameIndex;
        private float timer;

        private void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {

            if (rainFrames == null || rainFrames.Length == 0) return;


            timer += Time.deltaTime;

            if (timer >= 1f / frameRate)
            {
                timer -= 1f / frameRate; // Reset timer

                // Chuyển sang frame tiếp theo
                currentFrameIndex++;

                // (Loop)
                if (currentFrameIndex >= rainFrames.Length)
                {
                    currentFrameIndex = 0;
                }

                spriteRenderer.sprite = rainFrames[currentFrameIndex];
            }
        }

        public void SetSpeed(float newFrameRate)
        {
            frameRate = newFrameRate;
        }
    }
}
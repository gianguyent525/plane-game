using UnityEngine;

public enum GameDifficulty
{
    Easy,
    Normal,
    Hard
}

public class PlayerVisualController : MonoBehaviour
{
    public RuntimeAnimatorController easyController;
    public RuntimeAnimatorController normalController;
    public RuntimeAnimatorController hardController;

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        ApplyDifficulty(DifficultyManager.currentDifficulty);
    }

    public void ApplyDifficulty(GameDifficulty difficulty)
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null) return;
        }

        switch (difficulty)
        {
            case GameDifficulty.Easy:
                animator.runtimeAnimatorController = easyController;
                break;

            case GameDifficulty.Normal:
                animator.runtimeAnimatorController = normalController;
                break;

            case GameDifficulty.Hard:
                animator.runtimeAnimatorController = hardController;
                break;
        }
    }
}
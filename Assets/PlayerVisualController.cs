using UnityEngine;

public enum GameDifficulty
{
    Easy,
    Normal,
    Hard
}

public class PlayerVisualController : MonoBehaviour
{
    public GameDifficulty currentDifficulty;

    public RuntimeAnimatorController easyController;
    public RuntimeAnimatorController normalController;
    public RuntimeAnimatorController hardController;

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        ApplyDifficulty();
        Debug.Log("diffi " + currentDifficulty);
    }

    void ApplyDifficulty()
    {
        
        switch (currentDifficulty)
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
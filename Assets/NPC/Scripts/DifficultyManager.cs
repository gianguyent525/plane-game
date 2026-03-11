using UnityEngine;

//public enum GameDifficulty { Easy, Medium, Hard }

public class DifficultyManager
{
    // Mặc định là Medium
    public static GameDifficulty currentDifficulty = GameDifficulty.Normal;

    public static float GetSpeedMultiplier()
    {
        switch (currentDifficulty)
        {
            case GameDifficulty.Easy: return 0.8f;  // Quái chậm hơn
            case GameDifficulty.Normal: return 1.0f;
            case GameDifficulty.Hard: return 1.5f;  // Quái nhanh hơn
            default: return 1.0f;
        }
    }

    public static float GetFireRateMultiplier()
    {
        switch (currentDifficulty)
        {
            case GameDifficulty.Easy: return 1.5f;  // Ra quái chậm hơn
            case GameDifficulty.Normal: return 1.0f;
            case GameDifficulty.Hard: return 0.5f;  // Ra quái nhanh gấp đôi
            default: return 1.0f;
        }
    }

    public static int GetEnemyCount(int baseCount)
    {
        switch (currentDifficulty)
        {
            case GameDifficulty.Easy: return Mathf.Max(1, (int)(baseCount * 0.5f)); // Giảm nửa số lượng địch
            case GameDifficulty.Normal: return baseCount;
            case GameDifficulty.Hard: return (int)(baseCount * 2.0f); // Gấp đôi số lượng địch
            default: return baseCount;
        }
    }
}
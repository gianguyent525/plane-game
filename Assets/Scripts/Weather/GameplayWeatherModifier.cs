using UnityEngine;
public class GameplayWeatherModifier : MonoBehaviour
{
    public PlayerMove player;
    public void ApplyGameplayEffect(WeatherType type, float intensity)
    {
        switch (type)
        {
            case WeatherType.Clear:
                //BulletManager.Instance.SetAccuracy(0.85f);
                player.SetSpeedMultiplier(1f);
                break;

            case WeatherType.Rain:
                //PlayerController.Instance.SetSpeedMultiplier(4f);
                player.SetSpeedMultiplier(0.9f);
                break;

            case WeatherType.Clouds:
                //CameraController.Instance.SetVisibility(0.6f);
                player.SetSpeedMultiplier(1.2f);
                break;

            case WeatherType.Snow:
                //EnemySpawner.Instance.SetSpawnRate(1.3f);
                player.SetSpeedMultiplier(0.6f);
                break;
            case WeatherType.Thunderstorm:
                //BulletManager.Instance.SetAccuracy(0.7f);
                //PlayerController.Instance.SetSpeedMultiplier(0.7f);
                player.SetSpeedMultiplier(1f);
                break;
            case WeatherType.Atmosphere:
                //CameraController.Instance.SetVisibility(0.5f);
                player.SetSpeedMultiplier(0.7f);
                break;

            default:
                ResetGameplay();
                break;
        }
    }

    void ResetGameplay()
    {
        //BulletManager.Instance.SetAccuracy(1f);
        //PlayerController.Instance.SetSpeedMultiplier(1f);
    }
}

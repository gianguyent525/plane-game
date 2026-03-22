using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutController : MonoBehaviour
{
    [SerializeField] private string loginSceneName = "LoginScreen";

    public void OnLogoutClicked()
    {
        PlayerPrefs.DeleteKey("AuthToken");
        PlayerPrefs.DeleteKey("LoggedUsername");
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(loginSceneName);
    }
}

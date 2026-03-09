using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    // 
    private const string MOCK_USERNAME = "admin";
    private const string MOCK_PASSWORD = "123456";

    public void OnLoginClicked()
    {
        messageText.gameObject.SetActive(false);

        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Please enter username and password");
            return;
        }

        if (username == MOCK_USERNAME && password == MOCK_PASSWORD)
        {
            LoginSuccess();
        }
        else
        {
            ShowError("Invalid username or password");
        }
    }

    void LoginSuccess()
    {
        messageText.text = "Login success!";
        messageText.color = Color.green;
        messageText.gameObject.SetActive(true);

        // 
        Invoke(nameof(LoadGameplay), 0.5f);
    }

    void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    void ShowError(string msg)
    {
        messageText.text = msg;
        messageText.color = new Color32(255, 90, 90, 255);
        messageText.gameObject.SetActive(true);
    }
}

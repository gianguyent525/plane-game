using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    private string apiUrl = "http://localhost:8080/api/auth/login";

    public void OnLoginClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Please enter username and password");
            return;
        }

        StartCoroutine(Login(username, password));
    }

    IEnumerator Login(string username, string password)
    {
        LoginRequest requestData = new LoginRequest
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            LoginResponse response =
                JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            Debug.Log("Login success: " + response.username);

            // Save the username for later use (e.g., High Score API)
            PlayerPrefs.SetString("LoggedUsername", response.username);
            PlayerPrefs.Save();

            LoadGameplay();
        }
        else
        {
            Debug.LogError(request.error);
            ShowError("Invalid username or password");
        }
    }

    void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    void ShowError(string msg)
    {
        messageText.text = msg;
        messageText.color = Color.red;
        messageText.gameObject.SetActive(true);
    }
}
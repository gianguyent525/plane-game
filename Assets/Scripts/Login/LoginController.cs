using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ApiMessageResponse
{
    public string message;
}

public class LoginController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startMenuPanel;
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("Login UI")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    [Header("Register UI")]
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_InputField registerConfirmPasswordInput;
    public TMP_Text registerMessageText;

    [Header("API")]
    [SerializeField] private string loginApiUrl = "https://localhost:8080/api/Auth/login";
    [SerializeField] private string registerApiUrl = "https://localhost:8080/api/Auth/register";
    [SerializeField] private bool ignoreSslCertificateInEditor = true;

    [Header("Flow")]
    [SerializeField] private bool loadGameplayOnLogin = false;

    private void Start()
    {
        if (startMenuPanel == null)
        {
            if (loginPanel != null) loginPanel.SetActive(true);
            if (registerPanel != null) registerPanel.SetActive(false);
            ClearMessages();
            return;
        }

        ShowStartMenu();
    }

    public void OnOpenLoginPanelClicked()
    {
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (registerPanel != null) registerPanel.SetActive(false);
        if (loginPanel != null) loginPanel.SetActive(true);
        ClearMessages();
    }

    public void OnOpenRegisterPanelClicked()
    {
        if (startMenuPanel != null) startMenuPanel.SetActive(false);
        if (loginPanel != null) loginPanel.SetActive(false);
        if (registerPanel != null) registerPanel.SetActive(true);
        ClearMessages();
    }

    public void OnSwitchToRegisterClicked()
    {
        OnOpenRegisterPanelClicked();
    }

    public void OnSwitchToLoginClicked()
    {
        OnOpenLoginPanelClicked();
    }

    public void OnCloseAuthClicked()
    {
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        if (startMenuPanel == null)
        {
            if (loginPanel != null) loginPanel.SetActive(true);
            if (registerPanel != null) registerPanel.SetActive(false);
            ClearMessages();
            return;
        }

        if (startMenuPanel != null) startMenuPanel.SetActive(true);
        if (loginPanel != null) loginPanel.SetActive(false);
        if (registerPanel != null) registerPanel.SetActive(false);
        ClearMessages();
    }

    public void OnLoginClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Please enter username and password");
            return;
        }

        StartCoroutine(LoginCoroutine(username, password));
    }

    public void OnRegisterClicked()
    {
        var username = registerUsernameInput.text.Trim();
        var password = registerPasswordInput.text;
        var confirmPassword = registerConfirmPasswordInput != null ? registerConfirmPasswordInput.text : password;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowRegisterError("Please enter username and password");
            return;
        }

        if (password.Length != 6)
        {
            ShowRegisterError("Password must be exactly 6 characters");
            return;
        }

        if (password != confirmPassword)
        {
            ShowRegisterError("Password confirmation does not match");
            return;
        }

        StartCoroutine(RegisterCoroutine(username, password));
    }

    private IEnumerator LoginCoroutine(string username, string password)
    {
        var requestData = new LoginRequest
        {
            username = username,
            password = password
        };

        var json = JsonUtility.ToJson(requestData);
        var request = new UnityWebRequest(loginApiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        ConfigureCertificateHandling(request);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            if (response == null || string.IsNullOrWhiteSpace(response.token))
            {
                ShowError("Login failed: token is empty");
                yield break;
            }

            PlayerPrefs.SetString("AuthToken", response.token);
            PlayerPrefs.SetString("LoggedUsername", username);
            PlayerPrefs.Save();

            ShowSuccess("Login successful");

            if (loadGameplayOnLogin)
            {
                LoadGameplay();
            }
            else
            {
                ShowStartMenu();
            }
        }
        else
        {
            var apiMessage = TryGetApiMessage(request.downloadHandler.text);
            Debug.LogError("Login error: " + request.error + " | " + request.downloadHandler.text);
            ShowError(string.IsNullOrWhiteSpace(apiMessage) ? "Invalid username or password" : apiMessage);
        }
    }

    private IEnumerator RegisterCoroutine(string username, string password)
    {
        var requestData = new RegisterRequest
        {
            username = username,
            password = password
        };

        var json = JsonUtility.ToJson(requestData);
        var request = new UnityWebRequest(registerApiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        ConfigureCertificateHandling(request);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<RegisterResponse>(request.downloadHandler.text);
            var apiMessage = response != null ? response.message : string.Empty;
            ShowRegisterSuccess(string.IsNullOrWhiteSpace(apiMessage) ? "Register successful" : apiMessage);

            if (registerUsernameInput != null) registerUsernameInput.text = username;
            if (registerPasswordInput != null) registerPasswordInput.text = string.Empty;
            if (registerConfirmPasswordInput != null) registerConfirmPasswordInput.text = string.Empty;

            OnSwitchToLoginClicked();
            if (usernameInput != null) usernameInput.text = username;
        }
        else
        {
            var apiMessage = TryGetApiMessage(request.downloadHandler.text);
            Debug.LogError("Register error: " + request.error + " | " + request.downloadHandler.text);
            ShowRegisterError(string.IsNullOrWhiteSpace(apiMessage) ? "Register failed" : apiMessage);
        }
    }

    private void ConfigureCertificateHandling(UnityWebRequest request)
    {
#if UNITY_EDITOR
        if (ignoreSslCertificateInEditor)
        {
            request.certificateHandler = new DevCertificateHandler();
            request.disposeCertificateHandlerOnDispose = true;
        }
#endif
    }

    private static string TryGetApiMessage(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return string.Empty;
        }

        var parsed = JsonUtility.FromJson<ApiMessageResponse>(json);
        return parsed != null ? parsed.message : string.Empty;
    }

    private void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void ClearMessages()
    {
        if (messageText != null)
        {
            messageText.text = string.Empty;
        }

        if (registerMessageText != null)
        {
            registerMessageText.text = string.Empty;
        }
    }

    private void ShowError(string msg)
    {
        if (messageText == null) return;
        messageText.text = msg;
        messageText.color = Color.red;
        messageText.gameObject.SetActive(true);
    }

    private void ShowSuccess(string msg)
    {
        if (messageText == null) return;
        messageText.text = msg;
        messageText.color = Color.green;
        messageText.gameObject.SetActive(true);
    }

    private void ShowRegisterError(string msg)
    {
        if (registerMessageText == null) return;
        registerMessageText.text = msg;
        registerMessageText.color = Color.red;
        registerMessageText.gameObject.SetActive(true);
    }

    private void ShowRegisterSuccess(string msg)
    {
        if (registerMessageText == null) return;
        registerMessageText.text = msg;
        registerMessageText.color = Color.green;
        registerMessageText.gameObject.SetActive(true);
    }
}

public class DevCertificateHandler : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
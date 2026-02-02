using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LoginController : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    [Header("API")]
    public string loginUrl = "http://localhost:8080/api/auth/login";

    public void OnLoginClicked()
    {
        StartCoroutine(LoginCoroutine());
    }

    IEnumerator LoginCoroutine()
    {
        messageText.gameObject.SetActive(false);

        // 
        if (string.IsNullOrEmpty(usernameInput.text) || passwordInput.text.Length < 6)
        {
            ShowError("Invalid username or password");
            yield break;
        }

        // 
        LoginRequest requestData = new LoginRequest
        {
            username = usernameInput.text,
            password = passwordInput.text
        };

        string json = JsonUtility.ToJson(requestData);

        // 
        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 
        yield return request.SendWebRequest();

        // 
        if (request.result != UnityWebRequest.Result.Success)
        {
            ShowError("Server error");
            Debug.LogError(request.error);
            yield break;
        }

        Debug.Log("Response: " + request.downloadHandler.text);
        Debug.Log("RAW RESPONSE = " + request.downloadHandler.text);


        string raw = request.downloadHandler.text;
        Debug.Log("RAW = " + raw);

        LoginResponse response = JsonUtility.FromJson<LoginResponse>(raw);

        Debug.Log("username = " + response.username);
        Debug.Log("message = " + response.message);


        // 
        if (response.message.ToLower().Contains("success"))
        {
            messageText.text = "Welcome " + response.username;
            messageText.color = Color.green;
            messageText.gameObject.SetActive(true);

            // 
            // 
        }
        else
        {
            ShowError(response.message);
        }
    }

    void ShowError(string msg)
    {
        messageText.text = msg;
        messageText.color = new Color32(255, 107, 107, 255);
        messageText.gameObject.SetActive(true);
    }
}

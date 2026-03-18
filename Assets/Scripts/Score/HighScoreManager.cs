using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking;

[System.Serializable]
public class HighScoreRequest
{
    public string username;
    public int score;
}

[System.Serializable]
public class HighScoreResponse
{
    public string username;
    public int score;
}

[System.Serializable]
public class HighScoreList
{
    public HighScoreResponse[] items;
}

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }

    [Header("UI Bảng Xếp Hạng")]
    public TMP_Text highScoreTextUI;

    private string baseUrl = "http://localhost:8080/api/highscores";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SubmitScore(int score)
    {
        string username = PlayerPrefs.GetString("LoggedUsername", "");
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("Cannot submit score: No username logged in.");
            return;
        }

        StartCoroutine(SendScoreCoroutine(username, score));
    }

    private IEnumerator SendScoreCoroutine(string username, int score)
    {
        HighScoreRequest requestData = new HighScoreRequest
        {
            username = username,
            score = score
        };

        string json = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest(baseUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score submitted successfully for " + username + ": " + score);
        }
        else
        {
            Debug.LogError("Error submitting score: " + request.error + " - " + request.downloadHandler.text);
        }
    }

    public void GetTopScores()
    {
        StartCoroutine(GetTopScoresCoroutine());
    }

    private IEnumerator GetTopScoresCoroutine()
    {
        if (highScoreTextUI != null)
        {
            highScoreTextUI.text = "Đang tải Bảng Xếp Hạng...";
        }

        UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/top");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            // Unity bắt buộc bọc Json Array lại thành Object để đọc được
            string wrappedJson = "{\"items\":" + json + "}";
            HighScoreList list = JsonUtility.FromJson<HighScoreList>(wrappedJson);

            if (list != null && list.items != null && highScoreTextUI != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("--- BẢNG VÀNG THÀNH TÍCH ---");
                for (int i = 0; i < list.items.Length; i++)
                {
                    sb.AppendLine((i + 1) + ". " + list.items[i].username + " - " + list.items[i].score);
                }
                highScoreTextUI.text = sb.ToString();

                // Lấy điểm hạng 1 gán vào ô Điểm cao Kỷ lục trên bảng Game Over
                if (list.items.Length > 0 && ScoreManager.Instance != null && ScoreManager.Instance.topScoreText != null)
                {
                    ScoreManager.Instance.topScoreText.text = "High Score: " + list.items[0].score;
                }
            }
        }
        else
        {
            Debug.LogError("Lỗi tải bảng xếp hạng: " + request.error);
            if (highScoreTextUI != null)
            {
                highScoreTextUI.text = "Lỗi kết nối máy chủ!";
            }
        }
    }
}

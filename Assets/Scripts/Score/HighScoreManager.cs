using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class SubmitScoreRequest
{
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

    [Header("API")]
    [SerializeField] private string apiBaseUrl = "https://localhost:8080";
    [SerializeField] private int leaderboardTop = 10;
    [SerializeField] private bool ignoreSslCertificateInEditor = true;

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
        StartCoroutine(SendScoreCoroutine(score));
    }

    public void SubmitScoreAndRefresh(int score)
    {
        StartCoroutine(SubmitScoreAndRefreshCoroutine(score));
    }

    private IEnumerator SubmitScoreAndRefreshCoroutine(int score)
    {
        yield return SendScoreCoroutine(score);
        yield return GetTopScoresCoroutine();
    }

    private IEnumerator SendScoreCoroutine(int score)
    {
        var token = PlayerPrefs.GetString("AuthToken", "");
        if (string.IsNullOrWhiteSpace(token))
        {
            Debug.LogWarning("Cannot submit score: missing auth token.");
            yield break;
        }

        SubmitScoreRequest requestData = new SubmitScoreRequest
        {
            score = score
        };

        string json = JsonUtility.ToJson(requestData);
        var submitUrl = apiBaseUrl + "/api/Game/submit-score";

        UnityWebRequest request = new UnityWebRequest(submitUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + token);
        ConfigureCertificateHandling(request);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score submitted successfully: " + score);
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

        var leaderboardUrl = apiBaseUrl + "/api/Game/leaderboard?top=" + leaderboardTop;
        UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl);
        ConfigureCertificateHandling(request);
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
                sb.AppendLine("--- LeaderBoard ---");
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
}

using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class LeaderboardEntry
{
    public string username;
    public int score;
}

[System.Serializable]
public class LeaderboardEntryList
{
    public LeaderboardEntry[] items;
}

public class LeaderboardController : MonoBehaviour
{
    [Header("API")]
    [SerializeField] private string apiBaseUrl = "https://localhost:8080";
    [SerializeField] private int top = 10;

    [Header("UI")]
    [SerializeField] private TMP_Text leaderboardText;
    [SerializeField] private TMP_Text statusText;

    private void Start()
    {
        RefreshLeaderboard();
    }

    public void RefreshLeaderboard()
    {
        StartCoroutine(GetLeaderboardCoroutine());
    }

    private IEnumerator GetLeaderboardCoroutine()
    {
        if (leaderboardText != null)
        {
            leaderboardText.text = string.Empty;
        }

        SetStatus("Dang tai bang xep hang...");

        var url = $"{apiBaseUrl}/api/Game/leaderboard?top={top}";
        using var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            SetStatus("Khong the ket noi server");
            Debug.LogError("Load leaderboard failed: " + request.error + " | " + request.downloadHandler.text);
            yield break;
        }

        var wrappedJson = "{\"items\":" + request.downloadHandler.text + "}";
        var list = JsonUtility.FromJson<LeaderboardEntryList>(wrappedJson);

        if (list == null || list.items == null)
        {
            SetStatus("Du lieu leaderboard khong hop le");
            Debug.LogError("Invalid leaderboard JSON: " + request.downloadHandler.text);
            yield break;
        }

        RenderLeaderboard(list.items);
        SetStatus(string.Empty);
    }

    private void RenderLeaderboard(LeaderboardEntry[] entries)
    {
        if (leaderboardText == null)
        {
            return;
        }

        if (entries.Length == 0)
        {
            leaderboardText.text = "Chua co du lieu";
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("LEADERBOARD");

        for (var i = 0; i < entries.Length; i++)
        {
            var entry = entries[i];
            var username = string.IsNullOrWhiteSpace(entry.username) ? "Unknown" : entry.username;
            sb.AppendLine($"{i + 1}. {username} - {entry.score}");
        }

        leaderboardText.text = sb.ToString();
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}

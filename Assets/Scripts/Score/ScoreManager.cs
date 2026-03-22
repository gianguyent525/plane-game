using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }
    
    [Header("UI")]
    public TMP_Text scoreTextUI;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public TMP_Text topScoreText; // Điểm cao kỷ lục bên cạnh điểm hiện tại

    [Header("Leaderboard UI")]
    public GameObject leaderboardPanel; // Bảng chứa danh sách Leaderboard

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (scoreTextUI == null)
        {
            GameObject txtObj = GameObject.Find("ScoreText");
            if (txtObj != null)
            {
                scoreTextUI = txtObj.GetComponent<TMP_Text>();
            }
        }
        UpdateUI();
    }

    private void Update()
    {
        // Phím nóng gỡ lỗi: Nhấn phím 'R' trên bàn phím để ép game khởi động lại ngay lập tức!
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
        Debug.Log("Current Score: " + Score);
        UpdateUI();
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreTextUI != null)
        {
            scoreTextUI.text = "Score: " + Score.ToString();
        }
    }

    public void ShowGameOver()
    {
        // 1. Hiển thị bảng Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 2. Cập nhật Điểm số cuối cùng
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + Score.ToString();
        }

        // Tự động Tải Bảng Xếp Hạng từ Database Backend về
        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.GetTopScores();
        }

        // 3. Đóng băng thời gian trò chơi (Dừng mọi sự vật)
        Time.timeScale = 0f;
    }

    // Nút chức năng để Mở/Đóng bảng Xếp hạng
    public void ToggleLeaderboard(bool show)
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(show);
        }
    }

    public void OnContinueButtonClicked()
    {
        // restart game when click continue button
        RestartGame();
    }

    public void RestartGame()
    {
        // Nhả đóng băng thời gian và Tải lại màn chơi hiện tại
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UIElements;

public class DifficultyUIController : MonoBehaviour
{

    [Header("Liên kết Thanh Tiến Trình")]
    public ProgressBar progressBar; // Kéo script ProgressBar vào đây


    [Header("Liên kết UI Chính")]
    public TextMeshProUGUI intelText;
    public GameObject difficultyPanel;

    [Header("Liên kết Chữ trên Nút (Để đổi màu chữ)")]
    public TextMeshProUGUI txtEasy;
    public TextMeshProUGUI txtNormal;
    public TextMeshProUGUI txtHard;

    // --- BỔ SUNG MỚI: Liên kết tới script Glow của 3 nút ---
    [Header("Liên kết Hiệu ứng Glow (Để giữ sáng viền)")]
    public HDRButtonGlow glowEasy;
    public HDRButtonGlow glowNormal;
    public HDRButtonGlow glowHard;

    [Header("Màu sắc Chữ khi Highlight/Idle")]
    public Color colorIdle = new Color(0.5f, 0.6f, 0.5f); // Xám xanh (Chưa chọn)
    public Color colorEasy = new Color(0f, 1f, 0.5f);     // Xanh lục
    public Color colorNormal = new Color(1f, 0.7f, 0f);   // Vàng
    public Color colorHard = new Color(1f, 0.2f, 0.2f);   // Đỏ

    [Header("Liên kết Logic")]
    public WaveManager waveManager;
    public float typeSpeed = 0.02f;

    [Header("Nhạc nền")]
    public AudioSource bgmSource;

    [Header("Hồ sơ Tình báo")]
    [TextArea(3, 5)] public string easyText = ">>> HỒ SƠ: LÍNH MỚI <<<\n- Đe dọa: Thấp\n- Phản ứng: Chậm\n\n[ĐÁNH GIÁ]: An toàn.";
    [TextArea(3, 5)] public string normalText = ">>> HỒ SƠ: CHIẾN TUYẾN <<<\n- Đe dọa: Trung bình\n- Phản ứng: Nhanh\n\n[ĐÁNH GIÁ]: Cẩn thận đạn pháo.";
    [TextArea(3, 5)] public string hardText = ">>> HỒ SƠ: CẢM TỬ <<<\n- Đe dọa: TỐI ĐA\n- Tỉ lệ sống sót: 15%\n\n[ĐÁNH GIÁ]: Dành cho Ách chủ bài.";

    private Coroutine typingCoroutine;

    void Start()
    {
        intelText.text = "CHỜ LỆNH CHỈ HUY...\nHãy chọn một chiến dịch bên trái.";
        difficultyPanel.SetActive(true);

        // CẬP NHẬT: Vừa reset màu chữ, vừa reset đèn glow
        ResetAllVisuals();
    }

    // TÊN HÀM ĐƯỢC ĐỔI (từ ResetAllButtons thành ResetAllVisuals cho đúng ý nghĩa)
    // Dùng để đưa toàn bộ chữ và đèn về trạng thái chờ
    private void ResetAllVisuals()
    {
        // 1. Reset màu chữ (Code gốc của bạn)
        txtEasy.color = colorIdle;
        txtNormal.color = colorIdle;
        txtHard.color = colorIdle;

        // 2. Reset trạng thái giữ sáng của đèn Glow (Bổ sung mới)
        if (glowEasy != null) glowEasy.SetSelected(false);
        if (glowNormal != null) glowNormal.SetSelected(false);
        if (glowHard != null) glowHard.SetSelected(false);
    }

    public void SelectEasy()
    {
        DifficultyManager.currentDifficulty = GameDifficulty.Easy;

        ResetAllVisuals(); // Tắt hết chữ sáng và đèn sáng cũ

        // Bật highlight cho nút Easy
        txtEasy.color = colorEasy;       // Đổi màu chữ (Code gốc)
        if (glowEasy != null) glowEasy.SetSelected(true); // Giữ đèn sáng (Bổ sung mới)

        StartTyping(easyText);
    }

    public void SelectNormal()
    {
        DifficultyManager.currentDifficulty = GameDifficulty.Normal;

        ResetAllVisuals();

        txtNormal.color = colorNormal;
        if (glowNormal != null) glowNormal.SetSelected(true);

        StartTyping(normalText);
    }

    public void SelectHard()
    {
        DifficultyManager.currentDifficulty = GameDifficulty.Hard;

        ResetAllVisuals();

        txtHard.color = colorHard;
        if (glowHard != null) glowHard.SetSelected(true);

        StartTyping(hardText);
    }

    public void ConfirmMission()
    {
        // Bật nhạc nền khi start game
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }

        // 1. Ra lệnh cho WaveManager chạy
        if (waveManager != null)
        {
            waveManager.StartWaves();
        }

        // 2. RA LỆNH CHO THANH PROGRESS BẮT ĐẦU CHẠY
        if (progressBar != null)
        {
            progressBar.StartProgress();
        }

        // 3. Tắt Panel UI này đi
        difficultyPanel.SetActive(false);
    }

    private void StartTyping(string textToType)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(textToType));
    }

    private IEnumerator TypeText(string textToType)
    {
        intelText.text = "";
        foreach (char c in textToType.ToCharArray())
        {
            intelText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }


}
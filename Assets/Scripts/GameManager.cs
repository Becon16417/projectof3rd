using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("計時設定")]
    [SerializeField] private float gameTimer = 20f; // 初始倒數時間
    private float currentTimer;
    private bool isGameOver = false;

    [Header("UI 連結")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("遊戲結束 UI")]
    [SerializeField] private GameObject gameOverPanel; // 結束畫面面板
    [SerializeField] private TextMeshProUGUI winnerText; // 顯示誰贏了的文字

    void Start()
    {
        currentTimer = gameTimer;
        UpdateTimerUI();
    }

    void Update()
    {
        HandleTimer();

        // 快捷鍵重啟
        // KeyCode.Return 是主鍵盤的 Enter，KeyCode.KeypadEnter 是數字鍵盤的 Enter
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        // 確保時間縮放回到正常 
        Time.timeScale = 1f;

        // 重新載入當前啟用的場景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void HandleTimer()
    {
        if (isGameOver) return;

        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            UpdateTimerUI();

            if (currentTimer <= 0)
            {
                OnTimeUp();
            }
        }
    }

    // 當時間歸零時觸發
    void OnTimeUp()
    {
        Debug.Log("時間到！雙方扣除好感度。");

        // 搜尋場景中所有的 PlayerStats
        PlayerStats[] allPlayers = FindObjectsOfType<PlayerStats>();
        foreach (PlayerStats stats in allPlayers)
        {
            stats.AddAffection(-1); 
        }

        // 懲罰完後自動重製計時
        ResetTimer();
    }

    // 當任何一人送禮成功時呼叫此函式
    public void ResetTimer()
    {
        // 關鍵修正：使用變數而不是數字
        currentTimer = gameTimer;
        Debug.Log($"計時器已重製為：{gameTimer}秒");
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // 格式化為秒數與毫秒
            timerText.text = currentTimer.ToString("F1") + "s";

            // 當剩餘 5 秒時變紅色提醒
            timerText.color = currentTimer < 5f ? Color.red : Color.white;
        }
    }

    public void GameOver(int winnerID)
    {
        if (isGameOver) return; // 避免重複觸發

        isGameOver = true;
        Debug.Log($"遊戲結束！玩家 P{winnerID} 獲勝！");

        // 1. 停止倒數計時顯示
        if (timerText != null) timerText.text = "FINISHED!";

        // 2. 顯示勝利 UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            winnerText.text = $"P{winnerID} 贏得了好感！";
        }

        // 3. 凍結
        Time.timeScale = 0f; 
    }
}

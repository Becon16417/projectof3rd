using UnityEngine;
using UnityEngine.SceneManagement; // 控制場景切換的核心

public class StartMenuController : MonoBehaviour
{
    // 開始遊戲按鈕
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 設定按鈕 (目前先預留，之後可以做成顯示/隱藏選單面板)
    public void OpenSettings()
    {
        Debug.Log("開啟設定面板");
        // 你之後可以建立一個 SettingsPanel，然後在這裡用 .SetActive(true) 開啟它
    }

    // 離開遊戲按鈕
    public void QuitGame()
    {
        Debug.Log("玩家選擇離開遊戲");

        Application.Quit();
    }
}
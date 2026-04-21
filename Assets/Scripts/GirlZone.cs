using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private int playerID; // 設定為 1 或 2，區分這是誰的基地
    [SerializeField] private int scorePrice = 50; // 送禮需要的金錢

    private void OnTriggerEnter(Collider other)
    {
        // 檢查是否為對應的玩家進入
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            PlayerStats stats = other.GetComponent<PlayerStats>();

            // 只有正確的玩家 ID 才能在自己的基地得分
            if (controller != null && controller.GetPlayerID() == playerID)
            {
                if (stats.GetCurrentMoney() >= scorePrice)
                {
                    ExecuteScore(stats);
                }
                else
                {
                    Debug.Log($"P{playerID} 錢不夠！還差 {scorePrice - stats.GetCurrentMoney()} 元");
                }
            }
        }
    }

    // 在 GoalZone.cs 的 DeliverSuccess 或 ExecuteScore 內
    void ExecuteScore(PlayerStats stats)
    {
        stats.SpendMoney(scorePrice);
        stats.AddAffection(1);

        // 關鍵：重製全域計時器
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.ResetTimer();
        }
    }
}
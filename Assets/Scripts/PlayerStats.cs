using UnityEngine;
using TMPro;
using UnityEngine.UI; // 使用 Image 元件
using System.Collections.Generic; // 使用 List

public class PlayerStats : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private int playerID;
    [SerializeField] private int money = 0;
    [SerializeField] private int affection = 0; // 當前好感度 (0~5)

    [Header("UI 連結")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private List<GameObject> affectionSegments; // 拖入那 5 個格子

    void Start()
    {
        UpdateUI();
        UpdateAffectionUI();
    }

    public void AddAffection(int amount)
    {
        // Mathf.Clamp(當前值, 最小值, 最大值)
        affection = Mathf.Clamp(affection + amount, 0, 5);

        UpdateAffectionUI();
        Debug.Log($"{gameObject.name} 的好感度現在是: {affection}");
        if (affection >= 5)
        {
            // 取得 GameManager 並觸發遊戲結束，傳入自己的玩家 ID
            FindObjectOfType<GameManager>().GameOver(playerID);
        }
    }

    private void UpdateAffectionUI()
    {
        if (affectionSegments == null || affectionSegments.Count == 0) return;

        // 只有索引小於目前好感度的格子才會顯示
        for (int i = 0; i < affectionSegments.Count; i++)
        {
            if (affectionSegments[i] != null)
            {
                // 假設你有 5 格，affection=0 時，i < 0 恆為 false，全部隱藏
                // affection=1 時，只有 i=0 會顯示
                affectionSegments[i].SetActive(i < affection);
            }
        }
    }
    public void AddMoney(int amount)
    {
        // 如果目前錢已經是 0，且又被攻擊（amount 是負數），就直接跳出不執行
        if (money <= 0 && amount < 0) return;

        // 加上金額，但使用 Mathf.Max 確保結果最少是 0
        money = Mathf.Max(0, money + amount);

        UpdateUI();
        Debug.Log($"{gameObject.name} 現在有 {money} 元");
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
        UpdateUI();
    }

    public int GetCurrentMoney() => money;

    private void UpdateUI()
    {
        if (moneyText != null)
        {
            //$ 150
            moneyText.text = money.ToString();
        }
    }
}

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

    [Header("掉落設定")]
    [SerializeField] private GameObject coinPrefab; // 拖入你的金幣 Prefab
    [SerializeField] private int dropAmount = 10;   // 每次掉多少錢

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
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.GameOver(this.playerID);
            }
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
                // affection=0 時，i < 0 恆為 false，全部隱藏
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

    public void LoseMoneyOnAttack()
    {
        if (money >= dropAmount)
        {
            // 1. 扣除金錢
            money -= dropAmount;
            UpdateUI();

            // 2. 在玩家身邊生成金幣
            SpawnDroppedCoin();

            Debug.Log($"{gameObject.name} 被攻擊，掉落了 {dropAmount} 元！");
        }
    }

    private void SpawnDroppedCoin()
    {
        // 設定掉落位置：在玩家位置加上一個隨機的偏移量，避免金幣直接生在玩家腳下立刻又被吃掉
        Vector3 randomOffset = new Vector3(Random.Range(-80f, 80f), 10f, Random.Range(-80f, 80f));
        Vector3 dropPos = transform.position + randomOffset;

        // 生成金幣
        GameObject droppedCoin = Instantiate(coinPrefab, dropPos, coinPrefab.transform.rotation);

        // 讓生成器知道多了一個金幣，維持數量平衡
        CoinSpawner spawner = FindObjectOfType<CoinSpawner>();
        if (spawner != null)
        {
            spawner.NotifyManualCoinSpawn();
        }
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

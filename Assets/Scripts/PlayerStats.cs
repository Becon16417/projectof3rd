using UnityEngine;
using TMPro;
using UnityEngine.UI; // 使用 Image 元件
using System.Collections.Generic; // 使用 List

public class PlayerStats : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private int playerID;
    [SerializeField] private int money = 0;
    public int affection = 0; // 當前好感度 (0~5)

    [Header("商店加成設定")]
    public float coinMultiplier = 1.0f;

    [Header("UI 連結")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private List<GameObject> affectionSegments; // 拖入那 5 個格子

    [Header("掉落設定")]
    [SerializeField] private GameObject droppedCoinPrefab; // 拖入你的金幣 Prefab
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
                // affection=0 時，i < 0 false，全部隱藏
                // affection=1 時，只有 i=0 會顯示
                affectionSegments[i].SetActive(i < affection);
            }
        }
    }
    public void AddMoney(int amount)
    {
        if (money <= 0 && amount < 0) return;

        // 賺錢時才乘上倍率，扣錢時直接扣除
        if (amount > 0)
        {
            money = Mathf.Max(0, money + Mathf.RoundToInt(amount * coinMultiplier));
        }
        else
        {
            money = Mathf.Max(0, money + amount);
        }

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
        // 1. 在玩家頭頂生成會彈跳的物理金幣
        Vector3 spawnPos = transform.position + Vector3.up * 200f; //200 單位
         GameObject droppedCoin = Instantiate(droppedCoinPrefab, spawnPos, droppedCoinPrefab.transform.rotation);

        // 2. 噴發
        Rigidbody coinRb = droppedCoin.GetComponent<Rigidbody>();
        if (coinRb != null)
        {
            coinRb.linearVelocity = Vector3.zero;

            // 向上噴，並往左右隨機散開
            Vector3 pushForce = new Vector3(
                Random.Range(-150f, 150f),
                Random.Range(200f, 400f),
                Random.Range(-150f, 150f)
            );

            coinRb.AddForce(pushForce, ForceMode.Impulse);
        }

        // 3. 增加計數
        CoinSpawner spawner = FindObjectOfType<CoinSpawner>();
        if (spawner != null) spawner.NotifyManualCoinSpawn();
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

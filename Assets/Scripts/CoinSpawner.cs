using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("連結設定")]
    [SerializeField] private GameObject coinPrefab;    
    [SerializeField] private BoxCollider spawnArea;   

    [Header("生成參數")]
    [SerializeField] private int maxCoins = 10;        // 場面上最多金幣數
    [SerializeField] private float spawnRate = 2f;   // 生成速度 (秒)
    [SerializeField] private int initialCoins = 5;

    private int currentCoinCount = 0;
    private float nextSpawnTime;

    void Start()
    {
        // 遊戲開始
        for (int i = 0; i < initialCoins; i++)
        {
            // 檢查是否超過最大上限
            if (currentCoinCount < maxCoins)
            {
                SpawnCoin();
            }
        }

        // 設定下一次自動生成的點
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        // 如果時間到了，且金幣還沒滿
        if (Time.time >= nextSpawnTime && currentCoinCount < maxCoins)
        {
            SpawnCoin();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnCoin()
    {
        // 邊界
        Bounds bounds = spawnArea.bounds;

        // 隨機選點
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
        float spawnY = bounds.center.y + 60f; // 稍微浮在地面上

        Vector3 spawnPos = new Vector3(randomX, spawnY, randomZ);

        // 生成金幣
        Quaternion coinRotation = Quaternion.Euler(90f, 0f, 0f);

        // 生成時套用這個旋轉角度
        Instantiate(coinPrefab, spawnPos, coinRotation);
        currentCoinCount++;
    }

    // 當 Coin被吃掉時會呼叫這個
    public void OnCoinCollected()
    {
        currentCoinCount--;
    }

    public void NotifyManualCoinSpawn()
    {
        currentCoinCount++;
    }
}
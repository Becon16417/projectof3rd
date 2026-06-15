using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("生成設定")]
    public GameObject carPrefab;
    public float spawnInterval = 4f;

    [Header("發射方向設定")]
    public Vector3 spawnDirection = Vector3.forward;

    [Header("隨機車速設定")]
    public float minSpeed = 500f;  // 最慢車速
    public float maxSpeed = 900f; // 最快車速

    [Header("隨機生成時間設定 (秒)")]
    public float minSpawnInterval = 2f; // 最快生一台
    public float maxSpawnInterval = 6f; // 最慢生一台

    private float timer;
    private float currentRequiredInterval; // 當前這台車需要等待的目標時間

    void Start()
    {
        timer = Random.Range(0f, maxSpawnInterval);

        // 決定第一台車需要等待的目標時間
        SetNextRandomInterval();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentRequiredInterval)
        {
            SpawnCar();
            timer = 0f;

            // 車子生完後，決定下一台車要隔多久
            SetNextRandomInterval();
        }
    }

    void SpawnCar()
    {
        if (carPrefab == null) return;

        GameObject newCar = Instantiate(carPrefab, transform.position, Quaternion.identity);

        CarObstacle carScript = newCar.GetComponent<CarObstacle>();
        if (carScript != null)
        {
            // 隨機速度
            float randomSpeed = Random.Range(minSpeed, maxSpeed);

            // 把方向與這台車專屬的隨機速度一起傳過去
            carScript.Initialize(spawnDirection, randomSpeed);

            Debug.Log($"[車輛生成] 成功生出一輛時速 {randomSpeed:F1} 的車！");
        }
    }

    // 抽籤決定時間
    void SetNextRandomInterval()
    {
        currentRequiredInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, spawnDirection.normalized * 4f);
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
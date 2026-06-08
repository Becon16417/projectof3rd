using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("生成設定")]
    public GameObject carPrefab;
    public float spawnInterval = 4f;

    [Header("發射方向設定")]
    public Vector3 spawnDirection = Vector3.forward;

    // 在 Inspector 面板控制車速範圍
    [Header("隨機車速設定")]
    public float minSpeed = 500f;  // 最慢車速
    public float maxSpeed = 900f; // 最快車速

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCar();
            timer = 0f;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, spawnDirection.normalized * 4f);
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
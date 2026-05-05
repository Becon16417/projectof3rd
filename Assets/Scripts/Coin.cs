using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 10;
    [SerializeField] private GameObject moneyPopupPrefab; // 稍後製作的特效預製物

    void Update()
    {
        transform.Rotate(0f, 0f, 100f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 檢查撞到的是不是玩家（記得要把 Capsule 的 Tag 設為 "Player"）
        if (other.CompareTag("Player"))
        {
            
            other.GetComponent<PlayerStats>().AddMoney(coinValue);

            // 賺錢文字特效
            if (moneyPopupPrefab != null)
            {
                Instantiate(moneyPopupPrefab, transform.position + Vector3.up, Quaternion.identity);
            }

            CoinSpawner spawner = FindObjectOfType<CoinSpawner>();
            if (spawner != null)
            {
                spawner.OnCoinCollected();
            }
            //金幣消失
            Destroy(gameObject);
        }
    }
}

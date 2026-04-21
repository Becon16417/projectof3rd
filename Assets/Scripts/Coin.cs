using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 10;
    [SerializeField] private GameObject moneyPopupPrefab; // 稍後製作的特效預製物

    private void OnTriggerEnter(Collider other)
    {
        // 檢查撞到的是不是玩家（記得要把 Capsule 的 Tag 設為 "Player"）
        if (other.CompareTag("Player"))
        {
            // 1. 給玩家錢
            other.GetComponent<PlayerStats>().AddMoney(coinValue);

            // 2. 在金幣位置生成「賺錢文字」特效
            if (moneyPopupPrefab != null)
            {
                Instantiate(moneyPopupPrefab, transform.position + Vector3.up, Quaternion.identity);
            }

            // 3. 金幣消失
            Destroy(gameObject);
        }
    }
}

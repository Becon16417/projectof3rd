using UnityEngine;

public class BaseShop : MonoBehaviour
{
    [Header("商店設定")]
    [SerializeField] protected int requiredCost = 50; // 購買所需金幣量

    [Header("旋轉")]
    [SerializeField] private float rotateSpeed = 90f;  // 每秒旋轉的角度

    private void Update()
    {
        // 讓方塊沿著自己的 X 軸自轉
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                // 使用原本的 GetCurrentMoney() 檢查餘額
                if (playerStats.GetCurrentMoney() >= requiredCost)
                {
                    // 執行各商店的專屬效果
                    bool isSuccess = ApplyShopEffect(other.gameObject, playerStats);

                    if (isSuccess)
                    {
                        // 交易成功，使用原本的 SpendMoney() 扣除金幣
                        playerStats.SpendMoney(requiredCost);
                        Debug.Log($"{other.name} 成功購買 {gameObject.name}，扣除 {requiredCost} 元。");
                    }
                }
                else
                {
                    Debug.Log($"{other.name} 想買東西，但是錢不夠！需要 {requiredCost} 元。");
                }
            }
        }
    }

    // 虛擬函式，供 3 個商店覆寫效果
    protected virtual bool ApplyShopEffect(GameObject player, PlayerStats stats)
    {
        return true;
    }
}

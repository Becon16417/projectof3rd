using UnityEngine;

public class CoinBonusShop : BaseShop
{
    protected override bool ApplyShopEffect(GameObject player, PlayerStats stats)
    {
        // 每次踩到，加成永久提升 10% 賺錢量
        stats.coinMultiplier += 0.1f;
        Debug.Log($"{player.name} 金幣獲取量提升 10%！目前倍率：{stats.coinMultiplier}");
        return true; // 允許扣錢
    }
}
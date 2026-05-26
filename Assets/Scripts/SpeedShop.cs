using UnityEngine;

public class SpeedShop : BaseShop
{
    protected override bool ApplyShopEffect(GameObject player, PlayerStats stats)
    {
        PlayerController moveScript = player.GetComponent<PlayerController>();

        if (moveScript != null)
        {
            // 程式碼變得超級乾淨！直接乘上 1.1 倍
            moveScript.moveSpeed *= 1.1f;
            Debug.Log($"{player.name} 速度提升 10%！目前速度：{moveScript.moveSpeed}");
            return true; // 允許扣錢
        }

        return false;
    }
}

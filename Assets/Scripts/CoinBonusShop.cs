using UnityEngine;

public class CoinBonusShop : BaseShop
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cheerClip;
    public void Awake()
    {

        OnBuySuccess += PlayEffect;
    }
    private void OnDisable()
    {
        OnBuySuccess -= PlayEffect;
    }
    private void PlayEffect(GameObject player)
    {
        Animator playerAnim = player.GetComponent<Animator>();
        AudioSource audio = player.GetComponent<AudioSource>();

        if (playerAnim != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.StartCheer(); 
            }
        }
        if (audio != null && cheerClip != null)
        {
            audio.PlayOneShot(cheerClip);
        }
    }
    protected override bool ApplyShopEffect(GameObject player, PlayerStats stats)
    {
        // 每次踩到，加成永久提升 10% 賺錢量
        stats.coinMultiplier += 0.1f;
        Debug.Log($"{player.name} 金幣獲取量提升 10%！目前倍率：{stats.coinMultiplier}");

        return true; // 允許扣錢
    }
}
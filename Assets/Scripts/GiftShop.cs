using UnityEngine;

public class GiftShop : BaseShop
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cheerClip;
    public void Start()
    {
        OnBuySuccess += PlayEffect;
    }
    private void PlayEffect(GameObject player)
    {
        Animator playerAnim = player.GetComponent<Animator>();
        AudioSource audio = player.GetComponent<AudioSource>();

        if (playerAnim != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if(pc != null)
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
        if (stats.affection >= 5)
        {
            Debug.Log($"{player.name} 的好感度已達滿格（5格），禮物商店拒絕出售！");
            return false; // 回傳失敗，BaseShop 就不會扣除金幣
        }

        // 呼叫 AddAffection，處理亮 UI 格子與 GameManager.GameOver 判定！
        stats.AddAffection(1);

        // 送禮成功，呼叫 GameManager 重製計時器
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.ResetTimer();
        }

        return true; // 允許扣錢
    }
}

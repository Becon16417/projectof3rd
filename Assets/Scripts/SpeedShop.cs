using UnityEngine;

public class SpeedShop : BaseShop
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

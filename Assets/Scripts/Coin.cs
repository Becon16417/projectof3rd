using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("音效設定")]
    [SerializeField] private AudioClip coinSound;

    [SerializeField] private int coinValue = 10;
    [SerializeField] private GameObject moneyPopupPrefab;

    void Update()
    {
        transform.Rotate(0f, 0f, 100f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("成功撞到金幣了！");
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if(playerAudio != null && coinSound != null)
            {
                playerAudio.PlayOneShot(coinSound);
            }
            else if (playerAudio == null)
            {
                Debug.LogWarning("提示：玩家身上忘記掛載 AudioSource 組件了！");
            }

            other.GetComponent<PlayerStats>().AddMoney(coinValue);

            if (moneyPopupPrefab != null)
            {
                Instantiate(moneyPopupPrefab, transform.position + Vector3.up, Quaternion.identity);
            }

            CoinSpawner spawner = FindObjectOfType<CoinSpawner>();
            if (spawner != null) spawner.OnCoinCollected();

            Destroy(gameObject);
        }
       
    }
}

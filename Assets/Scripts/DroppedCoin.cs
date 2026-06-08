using UnityEngine;

public class DroppedCoin : MonoBehaviour
{
    [SerializeField] private int coinValue = 10;
    [SerializeField] private GameObject moneyPopupPrefab;

    void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void Update()
    {

        transform.Rotate(0f, 0f, 100f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddMoney(coinValue);
            }

            if (moneyPopupPrefab != null)
            {
                Instantiate(moneyPopupPrefab, transform.position + Vector3.up, Quaternion.identity);
            }

            CoinSpawner spawner = FindObjectOfType<CoinSpawner>();
            if (spawner != null)
            {
                spawner.OnCoinCollected();
            }

            Destroy(gameObject);
        }
    }
}
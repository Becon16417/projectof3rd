using UnityEngine;

public class Coin : MonoBehaviour
{
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

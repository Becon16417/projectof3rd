using UnityEngine;

public class CarObstacle : MonoBehaviour
{
    [Header("車輛巡航設定")]
    public float speed = 10f;
    public float lifeTime = 6f;

    [Header("撞擊擊飛設定")]
    public float knockbackSpeed = 18f;  // 水平後退速度
    public float knockupSpeed = 6f;     // 向上噴發速度
    public float carStunDuration = 1.5f;

    [Header("音效")]
    [SerializeField] private AudioClip hitSound;

    private Vector3 moveDirection;

    public void Initialize(Vector3 direction, float chosenSpeed)
    {
        moveDirection = direction.normalized;
        transform.forward = moveDirection;
        this.speed = chosenSpeed; // 讓這台車採用隨機速度
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerCtrl = other.GetComponent<PlayerController>();
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            AudioSource playerAudio = other.GetComponent<AudioSource>();

            if (playerCtrl != null && playerRb != null)
            {
                // 觸發暈眩與掉錢
                playerCtrl.GetHit(carStunDuration);

                // 玩家位置減去車子位置 = 從車子中心指向玩家的方向
                Vector3 knockbackDir = other.transform.position - transform.position;

                // 強制將 Y 軸歸零，只保留水平推力
                knockbackDir.y = 0f;
                knockbackDir = knockbackDir.normalized;

                // 組合水平後退速度與垂直噴高速度
                Vector3 finalVelocity = (knockbackDir * knockbackSpeed) + (Vector3.up * knockupSpeed);

                playerRb.linearVelocity = finalVelocity;

                if(playerAudio != null && hitSound != null)
                {
                    playerAudio.PlayOneShot(hitSound);
                }

                Debug.Log($"[相對位置擊飛] 玩家位置 {other.transform.position}，車子位置 {transform.position}，推開方向為：{knockbackDir}");
            }
        }
    }
}
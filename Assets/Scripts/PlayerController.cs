using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] private int playerID;
    public int GetPlayerID() => playerID;

    [Tooltip("Movement")]
    [SerializeField] [Range(500f, 800f)] private float moveSpeed = 600f;

    [Tooltip("Holding Gift Speed")]
    [SerializeField] private GameObject giftVisual; // 拖入剛才做的 Cube
    //[SerializeField] [Range(0.1f, 1f)] private float carrySpeedMultiplier = 0.6f;

    [Header("KeyCode")]
    [SerializeField] private KeyCode moveUp;
    [SerializeField] private KeyCode moveDown;
    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode attackKey;
    [SerializeField] private KeyCode giveKey;

    [Header("attack")]
    [SerializeField] private float attackRange = 200f; // 攻擊距離
    [SerializeField] [Range(80f, 120f)] private float attackRadius = 100f; // 攻擊判定半徑
    [SerializeField] [Range(0.5f, 5f)] private float stunDuration = 2.0f; // 暈眩時間

    private bool isCarryingGift = false;
    private bool isStunned;
    private float stunTimer = 0f;
    private Rigidbody rb;
    private Vector3 moveInput;
    public bool GetIsCarrying() => isCarryingGift;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- 關鍵修復：處理暈眩計時 ---
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
            }
            // 暈眩時強制速度歸零，不執行後續輸入與移動
            rb.linearVelocity = Vector3.zero;
            return;
        }

        GetInput();
        if (Input.GetKeyDown(attackKey))
        {
            PerformAttack();
        }
    }

    void FixedUpdate()
    {
        if (!isStunned)
        {
            ApplyMovement();
        }
    }

    void GetInput()
    {
        float x = 0;
        float z = 0;

        if (Input.GetKey(moveUp)) z = 1;
        else if (Input.GetKey(moveDown)) z = -1;

        if (Input.GetKey(moveLeft)) x = -1;
        else if (Input.GetKey(moveRight)) x = 1;

        moveInput = new Vector3(x, 0, z).normalized;
    }

    // 讓購買區呼叫的函式
    public void PickUpGift()
    {
        isCarryingGift = true;
        if (giftVisual != null) giftVisual.SetActive(true);
    }

    // 讓送禮區呼叫的函式
    public void DeliverGift()
    {
        isCarryingGift = false;
        if (giftVisual != null) giftVisual.SetActive(false);
    }


    void ApplyMovement()
    {
        if (isStunned) return; // 暈眩中不執行移動邏輯

        float currentSpeed = moveSpeed;
        rb.linearVelocity = moveInput * currentSpeed;

        if (moveInput != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveInput, Time.deltaTime * 15f);
        }
    }

    void HandleStun()
    {
        rb.linearVelocity = Vector3.zero; // 暈眩不能動
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {
            isStunned = false;
            Debug.Log($"玩家 {playerID} 恢復行動");
        }
    }

    void PerformAttack()
    {
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint, attackRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.CompareTag("Player"))
            {
                PlayerController opponent = hitCollider.GetComponent<PlayerController>();
                if (opponent != null)
                {
                    opponent.GetHit(stunDuration); // 現在只剩暈眩效果
                    Debug.Log($"{gameObject.name} 擊暈了對手！");
                }
            }
        }
    }
    // 被打中時觸發
    public void GetHit(float duration)
    {
        if (isStunned) return;
        isStunned = true;
        stunTimer = duration;
    }


    // 在 Scene 視窗畫出攻擊範圍 (方便你調整數值)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(attackPoint, attackRadius);
    }
}
